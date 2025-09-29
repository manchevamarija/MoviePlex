#nullable enable
using System;

namespace MoviesApp.Models
{

    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string? Genre { get; set; }
        public string? Plot { get; set; }
        public string? PosterUrl { get; set; }
        public string? ImdbId { get; set; }   
        public string? Note { get; set; }
        public WatchStatus Status { get; set; } = WatchStatus.None;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

}