using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    public class RoomSeatDTO
    {

        public string RowLabel { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public bool IsActive { get; set; }

        public string Type {get; set; } = string.Empty;
    }

    public class RoomSeatRequestDTO : RoomSeatDTO
    {
        public long Id { get; set; }
    }   

    public class RoomSeatUpdateDTO : RoomSeatRequestDTO {}
}