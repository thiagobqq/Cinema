using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movie.Infra.Data;


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

            return services;
        }
        
    }
}