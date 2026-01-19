using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Test : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Tickets module is working!");
        }
    }
}