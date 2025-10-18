#nullable enable
using MoviesApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApp.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(Guid id);
        void Add(Movie movie);
        void Update(Movie movie);
        void Remove(Movie movie);
        Task SaveChangesAsync();
    }
}