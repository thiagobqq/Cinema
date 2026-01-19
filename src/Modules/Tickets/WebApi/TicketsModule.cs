using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Services;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Domain.Interfaces.Services;
using Tickets.Infra.Data;
using Tickets.Infra.Repository;

namespace Tickets.WebApi
{
    public static class TicketsModule
    {
        public static IServiceCollection AddTicketsModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TicketsDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            return services;
        }

        public static async Task SeedTicketsDataAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var ticketsContext = scope.ServiceProvider.GetRequiredService<TicketsDbContext>();
                await ticketsContext.Database.MigrateAsync();
            }
        }
    }
}
