using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Interfaces.Repositories;
using Auth.Infra.Data;

namespace Auth.Infra.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;
        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto?> GetUserById(string id)
        {
            var user = await _context.Users.FindAsync(id);;
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Name = user.UserName!,
                Email = user.Email!,
                Id = user.Id
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            return await Task.FromResult(_context.Users.Select(user => new UserResponseDto
            {
                Name = user.UserName!,
                Email = user.Email!,
                Id = user.Id
            }));
            
        }
    }
}