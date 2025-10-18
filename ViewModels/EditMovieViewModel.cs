#nullable enable
using MoviesApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class EditMovieViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Plot { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public string ImdbId { get; set; } = string.Empty; 
        public string Note { get; set; } = string.Empty;
        public WatchStatus Status { get; set; }

        public required DateTime CreatedAt { get; set; }
    }

}