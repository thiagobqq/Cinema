using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Application.DTO
{
     public class ErrorMessageResponseDTO
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}