using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Interfaces.Services;
using Auth.Domain.Models;
using Auth.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        public readonly TokenService _tokenService;
        private readonly AuthDbContext _dbContext;
        public AuthService(TokenService tokenService, AuthDbContext db)
        {
            _dbContext = db;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> Login(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, AuthDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email!.ToLower());
            if (user == null)
                throw new Exception("Invalid credentials");


            var result = await signinManager.CheckPasswordSignInAsync(user, request.Password!, false);

            if (!result.Succeeded)
                throw new Exception("Invalid credentials");

            return new AuthResponseDTO
            {
                email = user!.Email,
                username = user.UserName,
                Token = await _tokenService.createToken(user),
                Id = user.Id
            };
        }

        public async Task<ErrorMessageResponseDTO> Register(UserManager<AppUser> userManager, RegisterDto request)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email!.ToLower());
            if (user != null)
                throw new Exception("Email already in use");

            var newUser = new AppUser
            {
                Email = request.Email!.ToLower(),
                UserName = request.Name!.ToLower()
            };


            var result = await userManager.CreateAsync(newUser, request.Password!);
            if (!result.Succeeded)            
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));  
                
                return new ErrorMessageResponseDTO
                {
                    Success = false,
                    Message = $"User creation failed: {errorMessages}"
                };
            }

            await _dbContext.SaveChangesAsync();

            return new ErrorMessageResponseDTO
            {
                Success = true,
                Message = "User created successfully"
            };
        }
             
    }
}