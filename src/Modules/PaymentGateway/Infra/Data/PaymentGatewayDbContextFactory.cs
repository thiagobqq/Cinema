using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentGateway.Infra.Data
{
    public class PaymentGatewayDbContextFactory : IDesignTimeDbContextFactory<PaymentGatewayDbContext>
    {
        public PaymentGatewayDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentGatewayDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Cinema;User Id=sa;Password=Receba123!;TrustServerCertificate=True");

            return new PaymentGatewayDbContext(optionsBuilder.Options);
        }
    }
}
