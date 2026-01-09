using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Repositories
{
    internal interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllFilms();
        Task<Film?> GetFilmById(long FilmId);
        Task AddFilm(Film Film);
        Task UpdateFilm(Film Film);
        Task DeleteFilm(long FilmId);
        
    }
}