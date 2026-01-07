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
    internal class AuthService : IAuthService
    {
        public readonly TokenService _tokenService;
        private readonly AuthDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        public AuthService(TokenService tokenService, AuthDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            _dbContext = db;
            _tokenService = tokenService;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public async Task<AuthResponseDTO> Login(AuthDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!.ToLower());
            if (user == null)
                throw new Exception("Invalid credentials");


            var result = await _signinManager.CheckPasswordSignInAsync(user, request.Password!, false);

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

        public async Task<ErrorMessageResponseDTO> Register(RegisterDto request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email!.ToLower());
            if (user != null)
                throw new Exception("Email already in use");

            var newUser = new AppUser
            {
                Email = request.Email!.ToLower(),
                UserName = request.Name!.ToLower()
            };


            var result = await _userManager.CreateAsync(newUser, request.Password!);
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

        public async Task<ErrorMessageResponseDTO> ForgotPassword(ForgotPasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null)
                return new ErrorMessageResponseDTO
                {
                    Success = false,
                    Message = "User not found"
                };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            return new ErrorMessageResponseDTO
            {
                Success = true,
                Message = "Password token: " + token
            };
        }

        public async Task<ErrorMessageResponseDTO> ResetPassword(changePasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null)
                return new ErrorMessageResponseDTO
                {
                    Success = false,
                    Message = "User not found"
                };


            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password!);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));

                return new ErrorMessageResponseDTO
                {
                    Success = false,
                    Message = $"Password reset failed: {errorMessages}"
                };
            }

            return new ErrorMessageResponseDTO
            {
                Success = true,
                Message = "Password reset successfully"
            };
        }
             
    }

    
}