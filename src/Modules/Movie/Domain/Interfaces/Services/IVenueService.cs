using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Services
{
    public interface IVenueService
    {
        Task<IEnumerable<VenueResponseDTO>> GetAllVenues();
        Task<VenueResponseDTO?> GetVenueById(long venueId);
        Task AddVenue(VenueDTO venue);
        Task UpdateVenue(VenueUpdateDTO venue);
        Task DeleteVenue(long venueId);
    }
}