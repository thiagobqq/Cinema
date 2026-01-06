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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        public AuthController(IAuthService authService, UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            _authService = authService;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> forgotPassword([FromBody] ForgotPasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            return Ok(new { token });
            
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> reset([FromBody] changePasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null)
                return BadRequest();

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token!));

            var a = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password!);

            return Ok(a);

        }
        
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Invalid payload");

                var response = await _authService.Register(_userManager, request);
                
                if (!response.Success)
                    return BadRequest(response.Message);

                return Ok(new
                {
                    message = "Registro realizado com sucesso",
                });
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

                var response = await _authService.Login(_userManager, _signinManager, request);
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