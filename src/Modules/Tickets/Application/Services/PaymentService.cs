using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.Application.DTO;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Domain.Interfaces.Services;
using Tickets.Domain.Models.impl;

namespace Tickets.Application.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;

        public PaymentService(IPaymentRepository repo) => _repo = repo;

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
                ErrorMessage = p.ErrorMessage
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
                ErrorMessage = payment.ErrorMessage
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
                ErrorMessage = p.ErrorMessage
            });
        }

        public async Task CreatePayment(PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
                TicketId = paymentDTO.TicketId,
                Amount = paymentDTO.Amount,
                PaymentMethod = paymentDTO.PaymentMethod,
                Status = Tickets.Domain.Enums.PaymentStatus.Pending,
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

            if (statusUpdate.Status == Tickets.Domain.Enums.PaymentStatus.Confirmed)
                payment.ConfirmedAt = DateTime.UtcNow;

            await _repo.UpdatePayment(payment);
        }

        public async Task DeletePayment(long paymentId)
        {
            await _repo.DeletePayment(paymentId);
        }
    }
}
