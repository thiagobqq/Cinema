using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    public class SessionSeatDTO
    {
        public long SessionId { get; set; }
        public long RoomSeatId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ReservedUntil { get; set; }
        public string? TicketCode { get; set; }
    }

    public class SessionSeatRequestDTO : SessionSeatDTO
    {
        public long Id { get; set; }
    }

    public class SessionSeatUpdateDTO : SessionSeatRequestDTO { }
}
