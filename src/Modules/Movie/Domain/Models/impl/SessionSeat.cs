using Movie.Domain.Enums;

namespace Movie.Domain.Models.impl
{
    internal class SessionSeat : Model
    {
        public long SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public string RowLabel { get; set; } = string.Empty;
        public int SeatNumber { get; set; }  

        public SeatStatus Status { get; set; } = SeatStatus.Available;
        
        public DateTime? ReservedUntil { get; set; }
        
    }

    
}