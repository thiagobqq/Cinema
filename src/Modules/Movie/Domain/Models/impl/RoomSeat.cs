using Movie.Domain.Enums;

namespace Movie.Domain.Models.impl
{
   
    internal class RoomSeat : Model
    {
        public long RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public string RowLabel { get; set; } = string.Empty; 
        public int SeatNumber { get; set; }                    

        public SeatType Type { get; set; } = SeatType.Standard;
        
        public bool IsActive { get; set; } = true;  
        public ICollection<SessionSeat> SessionSeats { get; set; } = new List<SessionSeat>();
    }
}