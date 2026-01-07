using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(AuthDTO request);
        Task<ErrorMessageResponseDTO> Register(RegisterDto request);

        Task<ErrorMessageResponseDTO> ForgotPassword(ForgotPasswordDTO request);
        Task<ErrorMessageResponseDTO> ResetPassword(changePasswordDTO request);
    }
}