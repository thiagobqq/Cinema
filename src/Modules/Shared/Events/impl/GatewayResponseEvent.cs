using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Events;

namespace Shared.Events.impl
{
    public class GatewayResponseEvent : Event
    {
        public long SessionSeatId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public string TicketCode { get; set; } = string.Empty;
        
    }
}