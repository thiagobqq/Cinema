using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    internal class RoomDTO
    {
        public string Name { get; set; } = string.Empty;
        public long VenueId { get; set; }
        
    }

    internal class RoomResponseDTO : RoomDTO
    {
        public long Id { get; set; }
    }

    internal class RoomUpdateDTO : RoomResponseDTO{}
}