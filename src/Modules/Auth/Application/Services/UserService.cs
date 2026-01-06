using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Interfaces.Repositories;
using Auth.Domain.Interfaces.Services;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> GetUserById(string targetId)
        {
            return await _userRepository.GetUserById(targetId);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<bool> ChangePassword(string userId, UserManager<AppUser> userManager, UserUpdatePasswordDTO request)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);            
            Console.WriteLine(result);
            
            if (result.Succeeded)
                return true;           
            return false;
        }  
        
    }
}