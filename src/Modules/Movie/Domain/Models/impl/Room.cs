using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Domain.Models.impl
{
    internal class Room : Model
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public long VenueId { get; set; }
        public Venue Venue { get; set; } = null!;
        
    }
}