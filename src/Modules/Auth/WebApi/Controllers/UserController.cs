using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Application.DTO;
using Auth.Domain.Interfaces.Services;
using Auth.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Auth.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user =  await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [Authorize]
        [HttpGet("listAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("me/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserUpdatePasswordDTO request)
        {
            var response = await _userService.ChangePassword(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request);
            if(response)
                return Ok(new { message = "Password changed successfully" });
            return BadRequest(new { message = "Error changing password" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("admin/updateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserUpdateRoleDTO request)
        {
            var response = await _userService.UpdateUserRole(request);
            return response.Success ? Ok(new { response.Message }) : BadRequest(new { response.Message});
            
        }

    }
}