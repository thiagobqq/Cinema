using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;

namespace Movie.Application.Services
{
    internal class FilmService : IFilmService
    {
        private readonly IFilmRepository _repo;

        public FilmService(IFilmRepository repo) => _repo = repo;
        

        public async Task<IEnumerable<FilmResponseDTO>> GetAllFilms()
        {
            var films = await _repo.GetAllFilms();

            return  films.Select(f => new FilmResponseDTO
            {
                Id = f.Id,
                Title = f.Title,
                Rating = f.Rating,
                Description = f.Description,
                DurationMinutes = f.DurationMin,
                ReleaseDate = f.ReleaseDate,
                Genre = f.Genre
            });
        }

        public async Task<FilmResponseDTO?> GetFilmById(long filmId)
        {
            var film = await _repo.GetFilmById(filmId);
            if (film == null) 
                throw new KeyNotFoundException("Film not found");

            return new FilmResponseDTO
            {
                Id = film.Id,
                Title = film.Title,
                Rating = film.Rating,
                Description = film.Description,
                DurationMinutes = film.DurationMin,
                ReleaseDate = film.ReleaseDate,
                Genre = film.Genre
            };
        }

        public async Task AddFilm(FilmDTO filmDTO)
        {
            Film film = new Film
            {
                Title = filmDTO.Title,
                Description = filmDTO.Description,
                DurationMin = filmDTO.DurationMinutes,
                ReleaseDate = filmDTO.ReleaseDate,
                Genre = filmDTO.Genre
            };

            await _repo.AddFilm(film);
            
        }

        public async Task UpdateFilm(FilmUpdateDTO filmDTO)
        {
            var film = await _repo.GetFilmById(filmDTO.Id);
            if (film == null)
                throw new KeyNotFoundException("Film not found");

            film.Title = filmDTO.Title ?? film.Title;
            film.Description = filmDTO.Description ?? film.Description;
            film.DurationMin = filmDTO.DurationMinutes != 0 ? filmDTO.DurationMinutes : film.DurationMin;
            film.ReleaseDate = filmDTO.ReleaseDate != default ? filmDTO.ReleaseDate : film.ReleaseDate;
            film.Genre = filmDTO.Genre ?? film.Genre;
            
            await _repo.AddFilm(film);     
        }
        
        public async Task UpdateFilmRating(FilmRatingUpdateDTO ratingDTO)
        {
            var film = await _repo.GetFilmById(ratingDTO.FilmId);
            if (film == null)
                throw new KeyNotFoundException("Film not found");

            film.Rating = ratingDTO.NewRating.ToString("0.0");
            await _repo.AddFilm(film);
        }
        

        public async Task DeleteFilm(long filmId)
        {
            await _repo.DeleteFilm(filmId);
        }
    }
}