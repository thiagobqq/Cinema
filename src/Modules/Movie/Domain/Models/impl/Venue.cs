using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Domain.Models.impl
{
    internal class Venue : Model
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        
    }
}