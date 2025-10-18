#nullable enable
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp.Repositories
{
    public class DbMovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public DbMovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Movie?> GetByIdAsync(Guid id)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
        }

        public void Remove(Movie movie)
        {
            _context.Movies.Remove(movie);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}