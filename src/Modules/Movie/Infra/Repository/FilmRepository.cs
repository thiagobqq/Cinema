using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Repository
{
    internal class FilmRepository : IFilmRepository
    {
        private readonly MovieDbContext _context;

        public FilmRepository(MovieDbContext context) => _context = context;

        public async Task<IEnumerable<Film>> GetAllFilms()
        {
            return await _context.Films
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Film?> GetFilmById(long filmId)
        {
            return await _context.Films
                .Include(f => f.Sessions)
                    .ThenInclude(s => s.SessionSeats)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == filmId);
        }

        public async Task AddFilm(Film film)
        {
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFilm(Film film)
        {
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFilm(long filmId)
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film != null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }
        }
    }
}