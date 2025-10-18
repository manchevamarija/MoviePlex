#nullable enable
using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int? Year { get; set; }
        public string? Plot { get; set; }
        public string? PosterUrl { get; set; }

        [Display(Name = "Статус на гледање")]
        public WatchStatus WatchStatus { get; set; } = WatchStatus.NotWatched;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
    
}