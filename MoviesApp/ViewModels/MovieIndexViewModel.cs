#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Models; // <<< додадено
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
    public class MovieIndexViewModel
    {
        public List<Movie> Movies { get; set; } = new();
        public string? SearchString { get; set; }
        public string? GenreFilter { get; set; }
        public SelectList? AvailableGenres { get; set; }
    }
}