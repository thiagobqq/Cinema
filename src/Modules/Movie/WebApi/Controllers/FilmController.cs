using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Services;

namespace Movie.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var films = await _filmService.GetAllFilms();
            return Ok(films);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var film = await _filmService.GetFilmById(id);
                return Ok(film);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] FilmDTO request)
        {
            await _filmService.AddFilm(request);
            return Ok(new { message = "Film created" });
        }

        [HttpPut("{id:long}")]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] FilmUpdateDTO request)
        {
            request.Id = id;
            try
            {
                await _filmService.UpdateFilm(request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id:long}/rating")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRating(long id, [FromBody] FilmRatingUpdateDTO request)
        {
            request.FilmId = id;
            try
            {
                await _filmService.UpdateFilmRating(request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _filmService.DeleteFilm(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
