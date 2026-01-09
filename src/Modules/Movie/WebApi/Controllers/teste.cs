using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movie.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class teste : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

    }
}