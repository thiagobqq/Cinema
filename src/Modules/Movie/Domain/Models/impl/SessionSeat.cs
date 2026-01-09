using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movie.Domain.Enums;

namespace Movie.Domain.Models.impl
{
    internal class SessionSeat : Model
    {
        public long SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public long RoomSeatId { get; set; }
        public RoomSeat RoomSeat { get; set; } = null!;


        public SeatStatus Status { get; set; }
        
        public DateTime? ReservedUntil { get; set; }  
        
        public string? TicketCode { get; set; }      

        [ConcurrencyCheck] 
        [Column(TypeName = "rowversion")] 
        public byte[] RowVersion { get; set; } = null!;
    }
}