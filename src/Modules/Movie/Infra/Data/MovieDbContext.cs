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
        public DbSet<RoomSeat> RoomSeats { get; set; } 
        public DbSet<Film> Films { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionSeat> SessionSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureVenue(modelBuilder);
            ConfigureRoom(modelBuilder);
            ConfigureRoomSeat(modelBuilder); 
            ConfigureFilm(modelBuilder);
            ConfigureSession(modelBuilder);
            ConfigureSessionSeat(modelBuilder);
        }

        private static void ConfigureVenue(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasMany(v => v.Rooms)
                      .WithOne(r => r.Venue)
                      .HasForeignKey(r => r.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ConfigureRoom(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(r => r.Name)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.HasMany(r => r.Sessions)
                      .WithOne(s => s.Room)
                      .HasForeignKey(s => s.RoomId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });
        }

        private static void ConfigureRoomSeat(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomSeat>(entity =>
            {
                entity.HasIndex(rs => new { rs.RowLabel, rs.SeatNumber })
                      .IsUnique();

                entity.Property(rs => rs.RowLabel)
                      .HasMaxLength(5) 
                      .IsRequired();

                entity.Property(rs => rs.Type)
                      .HasConversion<string>() 
                      .HasMaxLength(20); 

                entity.Property(rs => rs.IsActive)
                      .HasDefaultValue(true);
            });
        }

        private static void ConfigureFilm(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>(entity =>
            {
                entity.Property(f => f.Title)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.HasIndex(f => f.Title);

                entity.HasMany(f => f.Sessions)
                      .WithOne(s => s.Film)
                      .HasForeignKey(s => s.FilmId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureSession(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(s => s.Price)
                      .HasPrecision(18, 2);

                entity.HasIndex(s => s.StartsAt);

                entity.HasIndex(s => new { s.RoomId, s.StartsAt });

                entity.HasMany(s => s.SessionSeats) 
                      .WithOne(ss => ss.Session)
                      .HasForeignKey(ss => ss.SessionId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });
        }

        private static void ConfigureSessionSeat(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionSeat>(entity =>
            {
                entity.HasIndex(ss => new { ss.SessionId, ss.RoomSeatId })
                      .IsUnique();

                entity.Property(ss => ss.Status)
                      .HasConversion<string>() 
                      .HasMaxLength(20); 

                entity.Property(ss => ss.TicketCode)
                      .HasMaxLength(50);
                
                entity.Property(ss => ss.RowVersion)
                      .IsRowVersion()
                      .IsRequired(); 
                entity.Property(ss => ss.ReservedUntil)
                      .IsRequired(false); 

                entity.HasOne(ss => ss.RoomSeat)
                      .WithMany(rs => rs.SessionSeats)
                      .HasForeignKey(ss => ss.RoomSeatId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });
        }
    }
}