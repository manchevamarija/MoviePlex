#nullable enable
using System;
using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; } = new Movie();
    }
}
