using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Domain.Models.impl
{
    internal class Film : Model
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMin { get; set; }

        public string Genre { get; set; } = string.Empty;

        public string Rating { get; set; } = string.Empty;
    
        public ICollection<Session> Sessions { get; set; } = new List<Session>();

        public DateTime ReleaseDate { get; set; }
    }
}