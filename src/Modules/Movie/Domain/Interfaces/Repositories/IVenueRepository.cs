using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Repositories
{
    internal interface IVenueRepository
    {
        Task<IEnumerable<Venue>> GetAllVenues();
        Task<Venue?> GetVenueById(long venueId);
        Task AddVenue(Venue venue);
        Task UpdateVenue(Venue venue);
        Task DeleteVenue(long venueId);
        
    }
}