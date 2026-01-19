using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Domain.Models
{
    internal abstract class Model
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
