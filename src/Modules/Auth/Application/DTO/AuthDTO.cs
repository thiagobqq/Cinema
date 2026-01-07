using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Application.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; }  = default!;

        [Required]
        public string Password { get; set; }  = default!;   

    }


    public class AuthDTO
    {

        [Required]
        [EmailAddress]
        public string  Email { get; set; } = default!;

        [Required]

        public string  Password { get; set; } = default!;
    }

    public class AuthResponseDTO
    {
        public string  Token { get; set; } = default!;
        public string  email { get; set; } = default!;
        public string  username { get; set; } = default!;
        public string  Id { get; set; } = default!;

        
    }
    public class ForgotPasswordDTO
    {
    [Required]
    [EmailAddress]
    public string Email { get; set; }  = default!;
    }


    public class changePasswordDTO
    {
        [Required]
        public string  Token { get; set; }  = default!;

        [Required]
        public string  Email { get; set; } = default!;

        public string  Password { get; set; } = default!;
    }
}