using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;

namespace Movie.Application.Services
{
    internal class VenueService : IVenueService
    {
        private readonly IVenueRepository _repo;

        public VenueService(IVenueRepository repo) => _repo = repo;

        public  async Task<IEnumerable<VenueResponseDTO>> GetAllVenues()
        {
            var venues = await _repo.GetAllVenues();

            return venues.Select(v => new VenueResponseDTO
            {
                Id = v.Id,
                Name = v.Name,
                Adress = v.Address,
                City = v.City
            });
        }

        public async Task<VenueResponseDTO?> GetVenueById(long venueId)
        {
            var venue = await _repo.GetVenueById(venueId);

            return new VenueResponseDTO
            {
                Id = venue!.Id,
                Name = venue.Name,
                Adress = venue.Address,
                City = venue.City
            };
        }

        public async Task AddVenue(VenueDTO venue)
        {
            var newVenue = new Venue
            {
                Name = venue.Name,
                Address = venue.Adress,
                City = venue.City
            };
            await _repo.AddVenue(newVenue);
        }

        public async Task UpdateVenue(VenueUpdateDTO request)
        {
            var venue = await _repo.GetVenueById(request.Id);
            if (venue == null) 
                throw new KeyNotFoundException("Venue not found");

            venue.Name = request.Name ?? venue.Name;
            venue.Address = request.Adress ?? venue.Address;
            venue.City = request.City ?? venue.City;

            await _repo.UpdateVenue(venue);
        }
        public Task DeleteVenue(long venueId) => _repo.DeleteVenue(venueId);
    }
}