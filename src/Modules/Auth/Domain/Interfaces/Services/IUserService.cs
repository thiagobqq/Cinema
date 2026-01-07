using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetUserById(string id);
        Task<IEnumerable<UserResponseDto>> GetAllUsers();


        Task<bool> ChangePassword(string userId, UserUpdatePasswordDTO request);
         
    }
}