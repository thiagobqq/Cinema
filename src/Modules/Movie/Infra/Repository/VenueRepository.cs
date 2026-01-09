using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Repository
{
    internal class VenueRepository : IVenueRepository
    {
        private readonly MovieDbContext _context;

        public VenueRepository(MovieDbContext context) => _context = context;

        public async Task<IEnumerable<Venue>> GetAllVenues()
        {
            return await _context.Venues
                .AsNoTracking()
                .Include(v => v.Rooms)
                .ToListAsync();
        }

        public async Task<Venue?> GetVenueById(long venueId)
        {
            return await _context.Venues
                .Include(v => v.Rooms)
                    .ThenInclude(r => r.Seats)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == venueId);
        }

        public async Task AddVenue(Venue venue)
        {
            await _context.Venues.AddAsync(venue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVenue(Venue venue)
        {
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVenue(long venueId)
        {
            var venue = await _context.Venues.FindAsync(venueId);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }
        }
    }
}