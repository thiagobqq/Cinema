using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.Events.impl;
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
        private readonly ITicketRepository _ticketRepository;
        private const string GATEWAY_URL = "http://localhost:5000/api/gateway/process";

        public PaymentService(IPaymentRepository repo, ITicketRepository ticketRepository)
        {
            _repo = repo;
            _ticketRepository = ticketRepository;
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
                var gatewayRequest = new GatewayProcessPaymentRequestDTO
                {
                    ExternalReference = payment.Id,
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

                await SendToGatewayAsync(gatewayRequest);

                return new ProcessPaymentResponseDTO
                {
                    PaymentId = payment.Id,
                    Status = PaymentStatus.Processing,
                    Message = "Payment sent to gateway. Awaiting confirmation via webhook."
                };
            }
            catch (Exception ex)
            {
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

        private async Task SendToGatewayAsync(GatewayProcessPaymentRequestDTO request)
        {
            using var httpClient = new HttpClient();
            
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(GATEWAY_URL, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gateway returned {response.StatusCode}: {error}");
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
                var refundRequest = new GatewayRefundRequestDTO
                {
                    TransactionId = payment.GatewayTransactionId,
                    Amount = payment.Amount,
                    Reason = reason
                };

                var refundResponse = await SendRefundToGatewayAsync(refundRequest);

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

        private async Task<GatewayRefundResponseDTO> SendRefundToGatewayAsync(GatewayRefundRequestDTO request)
        {
            using var httpClient = new HttpClient();
            
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:5000/api/gateway/refund", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                return new GatewayRefundResponseDTO
                {
                    Success = false,
                    Message = $"Gateway returned {response.StatusCode}: {responseContent}"
                };
            }

            return JsonSerializer.Deserialize<GatewayRefundResponseDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new GatewayRefundResponseDTO { Success = false, Message = "Invalid response" };
        }

        private PaymentStatus MapGatewayStatus(int gatewayStatus)
        {
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

        public async Task ProcessWebhookAsync(GatewayWebhookDTO webhook)
        {
            var payment = await _repo.GetPaymentById(webhook.ExternalReference);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment not found: {webhook.ExternalReference}");
            }

            payment.GatewayTransactionId = webhook.TransactionId;
            payment.Status = MapGatewayStatus(webhook.Status);
            payment.ErrorMessage = webhook.Message;
            payment.PixQrCode = webhook.PixQrCode;
            payment.PixCopyPaste = webhook.PixCopyPaste;

            if (payment.Status == PaymentStatus.Confirmed)
            {
                payment.ConfirmedAt = DateTime.UtcNow;
            }

            await _repo.UpdatePayment(payment);     

            var ticket = await _ticketRepository.GetTicketById(payment.TicketId);
            ticket!.PaymentStatus = MapGatewayStatus(webhook.Status);
            await _ticketRepository.UpdateTicket(ticket);    

            var gatewayEvent = new GatewayResponseEvent{ SessionSeatId = payment.Ticket.SessionSeatId, UserId = payment.Ticket.UserId, Status = MapGatewayStatus(webhook.Status).ToString(), TicketCode = ticket!.Code };
            await gatewayEvent.Call();

        }
    }
}
