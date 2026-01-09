using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    internal class RoomSeats
    {

        public int RoomId { get; set; }
        public int SeatNumber { get; set; }
        public bool IsActive { get; set; }
    }
}