using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Domain.Models.impl;
using Tickets.Infra.Data;

namespace Tickets.Infra.Repository
{
    internal class TicketRepository : ITicketRepository
    {
        private readonly TicketsDbContext _context;

        public TicketRepository(TicketsDbContext context) => _context = context;

        public async Task<IEnumerable<Ticket>> GetAllTickets() => await _context.Tickets.ToListAsync();

        public async Task<Ticket?> GetTicketById(long ticketId) => await _context.Tickets.FindAsync(ticketId);

        public async Task<IEnumerable<Ticket>> GetTicketsByUserId(string userId) =>
            await _context.Tickets.Where(t => t.UserId == userId).ToListAsync();

        public async Task AddTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicket(long ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
