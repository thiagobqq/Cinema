using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.Application.DTO;

namespace Tickets.Domain.Interfaces.Services
{
    internal interface ITicketService
    {
        Task<IEnumerable<TicketResponseDTO>> GetAllTickets();
        Task<TicketResponseDTO?> GetTicketById(long ticketId);
        Task<IEnumerable<TicketResponseDTO>> GetTicketsByUserId(string userId);
        Task<TicketResponseDTO> CreateTicket(TicketDTO ticket);
        Task UpdateTicket(TicketUpdateDTO ticket);
        Task DeleteTicket(long ticketId);
    }
}
