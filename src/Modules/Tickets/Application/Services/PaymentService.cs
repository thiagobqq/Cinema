using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.Application.DTO;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Domain.Interfaces.Services;
using Tickets.Domain.Models.impl;

namespace Tickets.Application.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly IPaymentGatewayClient _gatewayClient;

        public PaymentService(IPaymentRepository repo, IPaymentGatewayClient gatewayClient)
        {
            _repo = repo;
            _gatewayClient = gatewayClient;
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetAllPayments()
        {
            var payments = await _repo.GetAllPayments();
            return payments.Select(p => new PaymentResponseDTO
            {
                Id = p.Id,
                TicketId = p.TicketId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,
                GatewayTransactionId = p.GatewayTransactionId,
                RequestedAt = p.RequestedAt,
                ConfirmedAt = p.ConfirmedAt,
                ErrorMessage = p.ErrorMessage,
                PixQrCode = p.PixQrCode,
                PixCopyPaste = p.PixCopyPaste
            });
        }

        public async Task<PaymentResponseDTO?> GetPaymentById(long paymentId)
        {
            var payment = await _repo.GetPaymentById(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            return new PaymentResponseDTO
            {
                Id = payment.Id,
                TicketId = payment.TicketId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                GatewayTransactionId = payment.GatewayTransactionId,
                RequestedAt = payment.RequestedAt,
                ConfirmedAt = payment.ConfirmedAt,
                ErrorMessage = payment.ErrorMessage,
                PixQrCode = payment.PixQrCode,
                PixCopyPaste = payment.PixCopyPaste
            };
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByTicketId(long ticketId)
        {
            var payments = await _repo.GetPaymentsByTicketId(ticketId);
            return payments.Select(p => new PaymentResponseDTO
            {
                Id = p.Id,
                TicketId = p.TicketId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,
                GatewayTransactionId = p.GatewayTransactionId,
                RequestedAt = p.RequestedAt,
                ConfirmedAt = p.ConfirmedAt,
                ErrorMessage = p.ErrorMessage,
                PixQrCode = p.PixQrCode,
                PixCopyPaste = p.PixCopyPaste
            });
        }

        public async Task CreatePayment(PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
                TicketId = paymentDTO.TicketId,
                Amount = paymentDTO.Amount,
                PaymentMethod = paymentDTO.PaymentMethod,
                Status = PaymentStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            await _repo.AddPayment(payment);
        }

        public async Task UpdatePaymentStatus(long paymentId, PaymentStatusUpdateDTO statusUpdate)
        {
            var payment = await _repo.GetPaymentById(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            payment.Status = statusUpdate.Status;
            payment.GatewayTransactionId = statusUpdate.GatewayTransactionId ?? payment.GatewayTransactionId;
            payment.ErrorMessage = statusUpdate.ErrorMessage ?? payment.ErrorMessage;

            if (statusUpdate.Status == PaymentStatus.Confirmed)
                payment.ConfirmedAt = DateTime.UtcNow;

            await _repo.UpdatePayment(payment);
        }

        public async Task<ProcessPaymentResponseDTO> ProcessPaymentAsync(ProcessPaymentDTO paymentDTO)
        {
            // Cria o pagamento localmente com status Pending
            var payment = new Payment
            {
                TicketId = paymentDTO.TicketId,
                Amount = paymentDTO.Amount,
                PaymentMethod = paymentDTO.PaymentMethod,
                Status = PaymentStatus.Processing,
                RequestedAt = DateTime.UtcNow
            };

            await _repo.AddPayment(payment);

            try
            {
                // Chama o gateway via HTTP
                var gatewayRequest = new GatewayProcessPaymentRequest
                {
                    ExternalReference = payment.Id.ToString(),
                    Amount = paymentDTO.Amount,
                    PaymentMethod = (int)paymentDTO.PaymentMethod,
                    CardInfo = paymentDTO.CardInfo != null ? new GatewayCardInfo
                    {
                        CardNumber = paymentDTO.CardInfo.CardNumber,
                        CardHolder = paymentDTO.CardInfo.CardHolder,
                        ExpirationDate = paymentDTO.CardInfo.ExpirationDate,
                        Cvv = paymentDTO.CardInfo.Cvv
                    } : null,
                    PixInfo = paymentDTO.PixInfo != null ? new GatewayPixInfo
                    {
                        PixKey = paymentDTO.PixInfo.PixKey
                    } : null
                };

                var gatewayResponse = await _gatewayClient.ProcessPaymentAsync(gatewayRequest);

                // Mapeia o status do gateway para o status local
                payment.Status = MapGatewayStatus(gatewayResponse.Status);
                payment.GatewayTransactionId = gatewayResponse.TransactionId;
                payment.ErrorMessage = gatewayResponse.Message;
                payment.PixQrCode = gatewayResponse.PixQrCode;
                payment.PixCopyPaste = gatewayResponse.PixCopyPaste;

                if (payment.Status == PaymentStatus.Confirmed)
                    payment.ConfirmedAt = DateTime.UtcNow;

                await _repo.UpdatePayment(payment);

                return new ProcessPaymentResponseDTO
                {
                    PaymentId = payment.Id,
                    GatewayTransactionId = gatewayResponse.TransactionId,
                    Status = payment.Status,
                    Message = gatewayResponse.Message,
                    PixQrCode = gatewayResponse.PixQrCode,
                    PixCopyPaste = gatewayResponse.PixCopyPaste
                };
            }
            catch (Exception ex)
            {
                // Em caso de erro, atualiza o status para Failed
                payment.Status = PaymentStatus.Failed;
                payment.ErrorMessage = $"Gateway error: {ex.Message}";
                await _repo.UpdatePayment(payment);

                return new ProcessPaymentResponseDTO
                {
                    PaymentId = payment.Id,
                    Status = PaymentStatus.Failed,
                    Message = ex.Message
                };
            }
        }

        public async Task<ProcessPaymentResponseDTO> RefundPaymentAsync(long paymentId, string? reason = null)
        {
            var payment = await _repo.GetPaymentById(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            if (string.IsNullOrEmpty(payment.GatewayTransactionId))
                throw new InvalidOperationException("Payment has no gateway transaction");

            if (payment.Status != PaymentStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed payments can be refunded");

            try
            {
                var refundRequest = new GatewayRefundRequest
                {
                    TransactionId = payment.GatewayTransactionId,
                    Amount = payment.Amount,
                    Reason = reason
                };

                var refundResponse = await _gatewayClient.RefundAsync(refundRequest);

                if (refundResponse.Success)
                {
                    payment.Status = PaymentStatus.Refunded;
                    await _repo.UpdatePayment(payment);
                }

                return new ProcessPaymentResponseDTO
                {
                    PaymentId = payment.Id,
                    GatewayTransactionId = payment.GatewayTransactionId,
                    Status = refundResponse.Success ? PaymentStatus.Refunded : payment.Status,
                    Message = refundResponse.Message
                };
            }
            catch (Exception ex)
            {
                return new ProcessPaymentResponseDTO
                {
                    PaymentId = payment.Id,
                    GatewayTransactionId = payment.GatewayTransactionId,
                    Status = payment.Status,
                    Message = $"Refund failed: {ex.Message}"
                };
            }
        }

        private PaymentStatus MapGatewayStatus(int gatewayStatus)
        {
            // Mapeamento baseado no enum TransactionStatus do gateway
            // 0=Pending, 1=Processing, 2=Approved, 3=Declined, 4=Error, 5=Refunded
            return gatewayStatus switch
            {
                0 => PaymentStatus.Pending,
                1 => PaymentStatus.Processing,
                2 => PaymentStatus.Confirmed,
                3 => PaymentStatus.Failed,
                4 => PaymentStatus.Failed,
                5 => PaymentStatus.Refunded,
                _ => PaymentStatus.Failed
            };
        }
    }
}
