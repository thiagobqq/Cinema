using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Events.impl;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Services;
using Tickets.Application.DTO;
using Tickets.Domain.Enums;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Domain.Interfaces.Services;
using Tickets.Domain.Models.impl;


namespace Tickets.Application.Services
{
    internal class TicketService : ITicketService
    {
        private readonly ITicketRepository _repo;
        private readonly ISessionSeatService _sessionSeatService;
        private readonly IPaymentService _paymentService;

        public TicketService(ITicketRepository repo, ISessionSeatService sessionSeatService, IPaymentService paymentService)
        {
            _repo = repo;
            _sessionSeatService = sessionSeatService;
            _paymentService = paymentService;
        }

        public async Task<ErrorMessageResponseDTO> BuyTicket(BuyTicketDTO buyTicketDTO, string userId)
        {
            var available = await _sessionSeatService.IsSeatAvailable(buyTicketDTO.SessionSeatId);
            if (!available)
                return new ErrorMessageResponseDTO { Success = false, Message = "Seat not available" };

            var preBuyEvent = new PreBuyEvent{ SessionSeatId = buyTicketDTO.SessionSeatId, UserId = userId };
            await preBuyEvent.Call();  

            var ticket = CreateTicket(new TicketDTO
            {
                UserId = userId,
                SessionSeatId = buyTicketDTO.SessionSeatId,
                PaymentMethod = buyTicketDTO.PaymentMethod,
                Amount = buyTicketDTO.Amount
            }).Result;


            await _paymentService.ProcessPaymentAsync(new ProcessPaymentDTO
            {
                TicketId = ticket.Id,
                Amount = ticket.Amount,
                PaymentMethod = ticket.PaymentMethod
            });                     
            

            return new ErrorMessageResponseDTO { Success = true, Message = "Seat available" };
        }

        public async Task<IEnumerable<TicketResponseDTO>> GetAllTickets()
        {
            var tickets = await _repo.GetAllTickets();
            return tickets.Select(t => new TicketResponseDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                SessionSeatId = t.SessionSeatId,
                Code = t.Code,
                PurchaseDate = t.PurchaseDate,
                PaymentMethod = t.PaymentMethod,
                PaymentStatus = t.PaymentStatus,
                Amount = t.Amount
            });
        }

        public async Task<TicketResponseDTO?> GetTicketById(long ticketId)
        {
            var ticket = await _repo.GetTicketById(ticketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            return new TicketResponseDTO
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                SessionSeatId = ticket.SessionSeatId,
                Code = ticket.Code,
                PurchaseDate = ticket.PurchaseDate,
                PaymentMethod = ticket.PaymentMethod,
                PaymentStatus = ticket.PaymentStatus,
                Amount = ticket.Amount
            };
        }

        public async Task<IEnumerable<TicketResponseDTO>> GetTicketsByUserId(string userId)
        {
            var tickets = await _repo.GetTicketsByUserId(userId);
            return tickets.Select(t => new TicketResponseDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                SessionSeatId = t.SessionSeatId,
                Code = t.Code,
                PurchaseDate = t.PurchaseDate,
                PaymentMethod = t.PaymentMethod,
                PaymentStatus = t.PaymentStatus,
                Amount = t.Amount
            });
        }

        public async Task<TicketResponseDTO> CreateTicket(TicketDTO ticketDTO)
        {
            var ticket = new Ticket
            {
                UserId = ticketDTO.UserId,
                SessionSeatId = ticketDTO.SessionSeatId,
                Code = GenerateTicketCode(),
                PurchaseDate = DateTime.UtcNow,
                PaymentMethod = ticketDTO.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                Amount = ticketDTO.Amount
            };

            await _repo.AddTicket(ticket);

            return new TicketResponseDTO
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                SessionSeatId = ticket.SessionSeatId,
                Code = ticket.Code,
                PurchaseDate = ticket.PurchaseDate,
                PaymentMethod = ticket.PaymentMethod,
                PaymentStatus = ticket.PaymentStatus,
                Amount = ticket.Amount
            };
        }

        public async Task UpdateTicket(TicketUpdateDTO ticketDTO)
        {
            var ticket = await _repo.GetTicketById(ticketDTO.Id);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket not found");

            ticket.PaymentMethod = ticketDTO.PaymentMethod;
            ticket.PaymentStatus = ticketDTO.PaymentStatus;
            ticket.Amount = ticketDTO.Amount;

            await _repo.UpdateTicket(ticket);
        }

        public async Task DeleteTicket(long ticketId)
        {
            await _repo.DeleteTicket(ticketId);
        }

        private static string GenerateTicketCode()
        {
            return $"TKT-{DateTime.UtcNow.Ticks}-{Random.Shared.Next(1000, 9999)}";
        }
    }
}
