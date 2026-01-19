using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    public class RoomDTO
    {
        public string Name { get; set; } = string.Empty;
        public long VenueId { get; set; }
        
    }

    public class RoomResponseDTO : RoomDTO
    {
        public long Id { get; set; }
    }

    public class RoomUpdateDTO : RoomResponseDTO{}
}