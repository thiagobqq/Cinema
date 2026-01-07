using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Application.DTO;

namespace Auth.Domain.Interfaces.Repositories
{
    internal interface IUserRepository
    {
        Task<UserResponseDto?> GetUserById(string id);
        Task<IEnumerable<UserResponseDto>> GetAllUsers();
    }
}