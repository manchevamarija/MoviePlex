using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
    public class MovieIndexViewModel
    {
        public List<Movie> LocalMovies { get; set; } = new();
        public List<CineplexxMovie> CineplexxMovies { get; set; } = new();

        public List<CineplexxMovie> UpcomingMovies { get; set; } = new();
    }
}