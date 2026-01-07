using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Interfaces.Services
{
    internal interface IAuthService
    {
        Task<AuthResponseDTO> Login(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, AuthDTO request);
        Task<ErrorMessageResponseDTO> Register(UserManager<AppUser> userManager, RegisterDto request);
    }
}