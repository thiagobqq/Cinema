using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Services
{
    public interface IFilmService
    {
        Task<IEnumerable<FilmResponseDTO>> GetAllFilms();
        Task<FilmResponseDTO?> GetFilmById(long filmId);
        Task AddFilm(FilmDTO film);
        Task UpdateFilm(FilmUpdateDTO film);
        Task DeleteFilm(long filmId);
        Task UpdateFilmRating(FilmRatingUpdateDTO ratingDTO);
    }
}