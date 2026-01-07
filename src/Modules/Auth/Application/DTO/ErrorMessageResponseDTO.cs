using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Application.DTO
{
    internal class ErrorMessageResponseDTO
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}