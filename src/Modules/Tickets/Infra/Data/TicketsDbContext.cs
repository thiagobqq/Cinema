using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Models.impl;
using Tickets.Domain.Enums;

namespace Tickets.Infra.Data
{
    internal class TicketsDbContext : DbContext
    {
        public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTicket(modelBuilder);
            ConfigurePayment(modelBuilder);
        }

        private static void ConfigureTicket(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.UserId)
                      .HasMaxLength(450)
                      .IsRequired();

                entity.Property(t => t.Code)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(t => t.PaymentMethod)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(t => t.PaymentStatus)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(t => t.Amount)
                      .HasPrecision(18, 2);

                entity.HasIndex(t => t.Code).IsUnique();
                entity.HasIndex(t => t.UserId);
                entity.HasIndex(t => t.SessionSeatId);

                entity.HasMany(t => t.Payments)
                      .WithOne(p => p.Ticket)
                      .HasForeignKey(p => p.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ConfigurePayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.PaymentMethod)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(p => p.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(p => p.Amount)
                      .HasPrecision(18, 2);

                entity.Property(p => p.GatewayTransactionId)
                      .HasMaxLength(250);

                entity.Property(p => p.ErrorMessage)
                      .HasMaxLength(500);

                entity.HasIndex(p => p.TicketId);
                entity.HasIndex(p => p.GatewayTransactionId);
            });
        }
    }
}
