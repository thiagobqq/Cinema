using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Domain.Enums
{
    internal enum SeatStatus
    {
        Available,  
        Locked,     
        Sold,       
        Unavailable 
    }
}