using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Enums;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Seeder
{
    internal class MovieSeeder
    {
        public static async Task SeedAsync(MovieDbContext context)
        {
            try
            {
                if (!await context.Venues.AnyAsync())
                {
                    var venues = new List<Venue>
                    {
                        new Venue
                        {
                            Name = "Cinema Downtown",
                            Address = "Rua Principal, 100",
                            City = "São Paulo"
                        },
                        new Venue
                        {
                            Name = "Cinema Shopping",
                            Address = "Av. Paulista, 1000",
                            City = "São Paulo"
                        }
                    };
                    await context.Venues.AddRangeAsync(venues);
                    await context.SaveChangesAsync();
                }

                if (!await context.Rooms.AnyAsync())
                {
                    var venue = await context.Venues.FirstAsync();
                    var rooms = new List<Room>
                    {
                        new Room { Name = "Sala 1", VenueId = venue.Id },
                        new Room { Name = "Sala 2", VenueId = venue.Id },
                        new Room { Name = "Sala 3 - Premium", VenueId = venue.Id }
                    };
                    await context.Rooms.AddRangeAsync(rooms);
                    await context.SaveChangesAsync();
                }

                if (!await context.RoomSeats.AnyAsync())
                {
                    var roomSeats = new List<RoomSeat>();
                    var rows = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

                    foreach (var row in rows)
                    {
                        for (int seatNum = 1; seatNum <= 10; seatNum++)
                        {
                            var seatType = SeatType.Standard;
                            
                            if ((row == "H" || row == "I" || row == "J") && (seatNum >= 4 && seatNum <= 7))
                                seatType = SeatType.VIP;
                            
                            if (row == "A" && seatNum == 10)
                                seatType = SeatType.Accessible;

                            roomSeats.Add(new RoomSeat
                            {
                                RowLabel = row,
                                SeatNumber = seatNum,
                                Type = seatType,
                                IsActive = true
                            });
                        }
                    }

                    await context.RoomSeats.AddRangeAsync(roomSeats);
                    await context.SaveChangesAsync();
                }

                if (!await context.Films.AnyAsync())
                {
                    var films = new List<Film>
                    {
                        new Film
                        {
                            Title = "Inception",
                            Description = "Um ladrão que rouba segredos corporativos...",
                            DurationMin = 148,
                            Genre = "Ficção Científica",
                            Rating = "8.8",
                            ReleaseDate = new DateTime(2010, 7, 16)
                        },
                        new Film
                        {
                            Title = "The Matrix",
                            Description = "Um programador de computador descobre a verdade...",
                            DurationMin = 136,
                            Genre = "Ficção Científica",
                            Rating = "8.7",
                            ReleaseDate = new DateTime(1999, 3, 31)
                        },
                        new Film
                        {
                            Title = "Interestelar",
                            Description = "Uma equipe de astronautas viaja através de um buraco de minhoca...",
                            DurationMin = 169,
                            Genre = "Ficção Científica",
                            Rating = "8.6",
                            ReleaseDate = new DateTime(2014, 11, 7)
                        }
                    };
                    await context.Films.AddRangeAsync(films);
                    await context.SaveChangesAsync();
                }

                if (!await context.Sessions.AnyAsync())
                {
                    var room = await context.Rooms.FirstAsync();
                    var film = await context.Films.FirstAsync();
                    var now = DateTime.UtcNow;

                    var sessions = new List<Session>
                    {
                        new Session
                        {
                            RoomId = room.Id,
                            FilmId = film.Id,
                            StartsAt = now.AddDays(1).AddHours(14),
                            EndsAt = now.AddDays(1).AddHours(16).AddMinutes(28),
                            Price = 30.00m
                        },
                        new Session
                        {
                            RoomId = room.Id,
                            FilmId = film.Id,
                            StartsAt = now.AddDays(1).AddHours(18),
                            EndsAt = now.AddDays(1).AddHours(20).AddMinutes(28),
                            Price = 35.00m
                        },
                        new Session
                        {
                            RoomId = room.Id,
                            FilmId = film.Id,
                            StartsAt = now.AddDays(2).AddHours(20),
                            EndsAt = now.AddDays(2).AddHours(22).AddMinutes(28),
                            Price = 40.00m
                        }
                    };
                    await context.Sessions.AddRangeAsync(sessions);
                    await context.SaveChangesAsync();
                }

                if (!await context.SessionSeats.AnyAsync())
                {
                    var sessions = await context.Sessions.ToListAsync();
                    var roomSeats = await context.RoomSeats.ToListAsync();

                    foreach (var session in sessions)
                    {
                        foreach (var roomSeat in roomSeats)
                        {
                            var sessionSeat = new SessionSeat
                            {
                                SessionId = session.Id,
                                RoomSeatId = roomSeat.Id,
                                Status = SeatStatus.Available,
                                ReservedUntil = null,
                                TicketCode = null,
                                RowVersion = new byte[8] 
                            };
                            await context.SessionSeats.AddAsync(sessionSeat);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao popular banco de dados: {ex.Message}", ex);
            }
        }
    }
}
