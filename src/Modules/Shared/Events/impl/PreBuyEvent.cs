using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Shared.Events;

namespace Cinema.Events.impl
{
    public class PreBuyEvent : Event
    {
        public long SessionSeatId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
    
}