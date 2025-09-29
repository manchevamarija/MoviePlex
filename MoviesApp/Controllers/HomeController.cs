#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MoviesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchTerm, string? genreFilter)
        {
            var moviesQuery = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                moviesQuery = moviesQuery.Where(m => !string.IsNullOrEmpty(m.Title) &&
                                                     EF.Functions.Like(m.Title, $"%{searchTerm}%"));
            }

            if (!string.IsNullOrWhiteSpace(genreFilter) && genreFilter != "Сите жанрови")
            {
                moviesQuery = moviesQuery.Where(m => !string.IsNullOrEmpty(m.Genre) &&
                                                     EF.Functions.Like(m.Genre, $"%{genreFilter}%"));
            }

            var movies = await moviesQuery.OrderByDescending(m => m.Year).ToListAsync();

            var genres = await _context.Movies
                .Select(m => m.Genre ?? "")
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
            genres.Insert(0, "Сите жанрови");

            var viewModel = new MovieIndexViewModel
            {
                Movies = movies,
                SearchString = searchTerm,
                GenreFilter = genreFilter,
                AvailableGenres = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(genres, genreFilter)
            };

            return View(viewModel);
        }
    }
}