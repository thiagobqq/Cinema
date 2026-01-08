using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Interfaces.Repositories;
using Auth.Domain.Models;
using Auth.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infra.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(AuthDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserResponseDto?> GetUserById(string id)
        {
            var user = await _context.Users
            .FindAsync(id);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Name = user.UserName!,
                Email = user.Email!,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!,
                Id = user.Id
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {            
            var query = from user in _context.Users
                        let roleName = (from userRole in _context.UserRoles
                                        join role in _context.Roles on userRole.RoleId equals role.Id
                                        where userRole.UserId == user.Id
                                        select role.Name).FirstOrDefault()
                        select new UserResponseDto
                        {
                            Id = user.Id,
                            Name = user.UserName!,
                            Email = user.Email!,
                            Role = roleName!
                        };

            return await query.ToListAsync();
        }
    }
}