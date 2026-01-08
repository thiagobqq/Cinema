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
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<UserResponseDto?> GetUserById(string targetId)
        {
            return await _userRepository.GetUserById(targetId);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<bool> ChangePassword(string userId,  UserUpdatePasswordDTO request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);            
            Console.WriteLine(result);
            
            if (result.Succeeded)
                return true;           
            return false;
        }  
        
        public async Task<ErrorMessageResponseDTO> UpdateUserRole(UserUpdateRoleDTO request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return new ErrorMessageResponseDTO { Success = false, Message = "Error removing user roles" };

            var addResult = await _userManager.AddToRoleAsync(user, request.NewRole);
            if (addResult.Succeeded)
                return new ErrorMessageResponseDTO { Success = true, Message = "User role updated successfully" };
            return new ErrorMessageResponseDTO { Success = false, Message = "Error adding user role" };
        }
    }
}