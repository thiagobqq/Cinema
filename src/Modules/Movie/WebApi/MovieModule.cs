using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movie.Application.Services;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Infra.Data;
using Movie.Infra.Repository;


namespace Movie.WebApi
{
    public static class MovieModule
    {
        public static IServiceCollection AddMovieModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            

            services.AddDbContext<MovieDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IRoomSeatService, RoomSeatService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ISessionSeatService, SessionSeatService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IVenueService, VenueService>();

            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IRoomSeatRepository, RoomSeatRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ISessionSeatRepository, SessionSeatRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IVenueRepository, VenueRepository>();

            return services;
        }
        
    }
}