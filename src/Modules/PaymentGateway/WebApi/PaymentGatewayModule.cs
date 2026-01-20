using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Application.Services;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infra.Data;
using PaymentGateway.Infra.Repository;

namespace PaymentGateway.WebApi
{
    public static class PaymentGatewayModule
    {
        public static IServiceCollection AddPaymentGatewayModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<PaymentGatewayDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IPaymentGatewayService, FakePaymentGatewayService>();

            return services;
        }

        public static async Task SeedPaymentGatewayDataAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PaymentGatewayDbContext>();
                await context.Database.MigrateAsync();
            }
        }
    }
}
