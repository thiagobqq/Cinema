using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    public class SessionDTO
    {
        public long RoomId { get; set; }
        public long FilmId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
    }

    public class SessionRequestDTO
    {
        public long Id { get; set; }
    }

    public class SessionUpdateDTO : SessionDTO
    {
        public long Id { get; set; }
    }

    public class SessionResponseDTO : SessionDTO
    {
        public long Id { get; set; }
    }
}