#nullable enable
using System;

namespace MoviesApp.ViewModels
{
    public class DeleteMovieViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public string? Genre { get; set; }
        public string? Plot { get; set; }
        public string? PosterUrl { get; set; }
    }
}