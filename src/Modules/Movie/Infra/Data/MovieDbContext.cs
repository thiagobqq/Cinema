using Microsoft.EntityFrameworkCore;
using Movie.Domain.Models.impl; 
using Movie.Domain.Enums;  

namespace Movie.Infra.Data
{
    internal class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionSeat> SessionSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SessionSeat>(entity =>
            {
                entity.HasIndex(s => new { s.SessionId, s.RowLabel, s.SeatNumber })
                      .IsUnique();

                entity.Property(s => s.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
                entity.HasOne(s => s.Session)
                      .WithMany(sess => sess.Seats)
                      .HasForeignKey(s => s.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(s => s.Price)
                      .HasPrecision(18, 2);

                entity.HasOne(s => s.Film)
                      .WithMany(f => f.Sessions)
                      .HasForeignKey(s => s.FilmId);

                entity.HasOne(s => s.Room)
                      .WithMany() 
                      .HasForeignKey(s => s.RoomId);
            });
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasOne(r => r.Venue)
                      .WithMany(v => v.Rooms)
                      .HasForeignKey(r => r.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}