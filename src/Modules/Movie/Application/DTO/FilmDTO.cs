using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Application.DTO
{
    public class FilmDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int DurationMinutes { get; set; }
        public string Genre { get; set; } = string.Empty;
        
    }

    public class FilmResponseDTO : FilmDTO
    {
        public long Id { get; set; }
        public string Rating { get; set; } = string.Empty;
    }

    public class FilmUpdateDTO : FilmResponseDTO{}

    public class FilmRatingUpdateDTO
    {
        public long FilmId { get; set; }
        public double NewRating { get; set; }
    }
}