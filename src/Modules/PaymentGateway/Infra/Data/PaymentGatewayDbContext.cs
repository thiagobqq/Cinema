using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Infra.Data
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options)
        {
        }

        internal DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("GatewayTransactions");
                
                entity.HasKey(t => t.Id);
                
                entity.Property(t => t.Id)
                    .HasMaxLength(32)
                    .IsRequired();

                entity.Property(t => t.ExternalReference)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(t => t.Amount)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(t => t.PaymentMethod)
                    .IsRequired();

                entity.Property(t => t.Status)
                    .IsRequired();

                entity.Property(t => t.CreatedAt)
                    .IsRequired();

                entity.Property(t => t.ErrorMessage)
                    .HasMaxLength(500);

                entity.Property(t => t.PixQrCode)
                    .HasMaxLength(1000);

                entity.Property(t => t.PixCopyPaste)
                    .HasMaxLength(500);

                entity.Property(t => t.RefundedAmount)
                    .HasPrecision(18, 2);

                entity.HasIndex(t => t.ExternalReference);
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.CreatedAt);
            });
        }
    }
}
