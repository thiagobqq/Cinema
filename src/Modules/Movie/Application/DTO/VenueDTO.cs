using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    internal class VenueDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        
    }

    internal class VenueResponseDTO : VenueDTO
    {
        public long Id { get; set; }
    }

    internal class VenueUpdateDTO : VenueResponseDTO{}
    
}