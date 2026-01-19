using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.Domain.Models.impl;

namespace Tickets.Domain.Interfaces.Repositories
{
    internal interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTickets();
        Task<Ticket?> GetTicketById(long ticketId);
        Task<IEnumerable<Ticket>> GetTicketsByUserId(string userId);
        Task AddTicket(Ticket ticket);
        Task UpdateTicket(Ticket ticket);
        Task DeleteTicket(long ticketId);
    }
}
