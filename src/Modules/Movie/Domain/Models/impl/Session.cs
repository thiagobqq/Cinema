using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Domain.Models.impl
{
    internal class Session : Model
    {        
        public long RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public long FilmId { get; set; }
        public Film Film { get; set; } = null!;
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public decimal Price { get; set; }

        public ICollection<SessionSeat> OccupiedSeats { get; set; } = new List<SessionSeat>();

        
    }
}