using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.Domain.Models;
using Auth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Auth.Application.DTO;

namespace Auth.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> forgotPassword([FromBody] ForgotPasswordDTO request)
        {
            var response = await _authService.ForgotPassword(request);            
            return response.Success ? Ok(response.Message) : BadRequest(response.Message);            
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> reset([FromBody] changePasswordDTO request)
        {
            var response = await _authService.ResetPassword(request);
            return response.Success ? Ok(response.Message) : BadRequest(response.Message);

        }
        
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Invalid payload");

                var response = await _authService.Register(request);
                
                return response.Success ? Ok(response.Message) : BadRequest(response.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Invalid payload");

                var response = await _authService.Login(request);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest("Credenciais erradas");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}