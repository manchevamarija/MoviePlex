#nullable enable
using MoviesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MoviesApp.Repositories
{
    public class InMemoryMovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new();

        public IEnumerable<Movie> GetAll() => _movies.OrderByDescending(m => m.CreatedAt);

        public Movie? GetById(Guid id) => _movies.FirstOrDefault(m => m.Id == id);

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(Movie movie) => _movies.Add(movie);

        public void Update(Movie movie)
        {
            var existing = GetById(movie.Id);
            if (existing != null)
            {
                existing.Title = movie.Title;
                existing.Year = movie.Year;
                existing.Genre = movie.Genre;
                existing.Plot = movie.Plot;
                existing.PosterUrl = movie.PosterUrl;
                existing.UpdatedAt = movie.UpdatedAt;
            }
        }

        public void Remove(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }


        public void Remove(Guid id)
        {
            var movie = GetById(id);
            if (movie != null)
                _movies.Remove(movie);
        }
    }
}