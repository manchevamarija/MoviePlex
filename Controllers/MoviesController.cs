#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services;
using MoviesApp.ViewModels;
using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CineplexxService _cineplexxService;

        public MoviesController(AppDbContext context, CineplexxService cineplexxService)
        {
            _context = context;
            _cineplexxService = cineplexxService;
        }

        // ---------------- INDEX ----------------
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? genreFilter)
        {
            var moviesQuery = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                moviesQuery = moviesQuery.Where(m => m.Title != null &&
                                                     EF.Functions.Like(m.Title, $"%{searchTerm}%"));
            }

            if (!string.IsNullOrWhiteSpace(genreFilter))
            {
                moviesQuery = moviesQuery.Where(m => m.Genre != null && m.Genre == genreFilter);
            }

            var localMovies = await moviesQuery
                .OrderByDescending(m => m.Year.HasValue)
                .ThenByDescending(m => m.Year)
                .ThenBy(m => m.Title)
                .ToListAsync();

            var cineplexxMovies = await _cineplexxService.GetMoviesAsync(DateTime.Now);
            if (cineplexxMovies == null || cineplexxMovies.Count == 0)
            {
                cineplexxMovies = await _cineplexxService.GetMoviesAsync(DateTime.Now.AddDays(1));
            }

            var upcomingMovies = await _cineplexxService.GetUpcomingMoviesAsync();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["GenreFilter"] = genreFilter;

            var vm = new MovieIndexViewModel
            {
                LocalMovies = localMovies,
                CineplexxMovies = cineplexxMovies,
                UpcomingMovies = upcomingMovies
            };

            return View(vm);
        }

        // ---------------- DETAILS FOR LOCAL MOVIE ----------------
        public async Task<IActionResult> Details(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

      // ----------------  UPDATE WATCH STATUS ----------------
[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateWatchStatus(Guid id, WatchStatus status)
{
    var movie = await _context.Movies.FindAsync(id);
    if (movie == null) return NotFound();

    movie.WatchStatus = status;
    await _context.SaveChangesAsync();

    TempData["Toast"] = "Статусот е успешно ажуриран.";
    return RedirectToAction(nameof(Details), new { id });
}

[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddFromCineplexx(string id)
{
    // Преземи ги сите Cineplexx филмови (денес или утре ако нема денес)
    var cineMovies = await _cineplexxService.GetMoviesAsync(DateTime.Now);
    if (cineMovies == null || cineMovies.Count == 0)
    {
        cineMovies = await _cineplexxService.GetMoviesAsync(DateTime.Now.AddDays(1));
    }

    // Најди го конкретниот филм по ID
    var cineMovie = cineMovies.FirstOrDefault(m => m.Id == id);
    if (cineMovie == null) return NotFound();

    // Провери дали веќе постои локално со ист наслов и година (да не дуплираш)
var exists = await _context.Movies
    .AnyAsync(m => m.Title == cineMovie.Title);

    if (exists)
    {
        TempData["Toast"] = "Филмот веќе постои во твојата листа.";
        return RedirectToAction(nameof(CineplexxDetails), new { id });
    }

    // Креирај нов локален Movie ентитет
    var newMovie = new Movie
    {
        Id = Guid.NewGuid(),
        Title = cineMovie.Title,
        Plot = cineMovie.DescriptionCalculated,
        PosterUrl = cineMovie.PosterImage,
        Genre = cineMovie.Genres != null ? string.Join(", ", cineMovie.Genres) : null,
        Year = null,
        CreatedAt = DateTime.UtcNow
    };

    _context.Movies.Add(newMovie);
    await _context.SaveChangesAsync();

    TempData["Toast"] = "Филмот е додаден во твојата листа.";
    return RedirectToAction(nameof(Details), new { id = newMovie.Id });
}

        // ---------------- DETAILS FOR CINEPLEXX MOVIE ----------------
        [HttpGet]
        public async Task<IActionResult> CineplexxDetails(string id)
        {
            var cineMovies = await _cineplexxService.GetMoviesAsync(DateTime.Now);
            if (cineMovies == null || cineMovies.Count == 0)
            {
                cineMovies = await _cineplexxService.GetMoviesAsync(DateTime.Now.AddDays(1));
            }

            var cineMovie = cineMovies.FirstOrDefault(m => m.Id == id);
            if (cineMovie == null)
                return NotFound();

            return View("CineplexDetails", cineMovie);
        }

        // ---------------- LOCAL CREATE FUNCTION ----------------
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid) return View(movie);

            var apiKey = "3c0096ac";
            using var client = new HttpClient();
            var url = $"https://www.omdbapi.com/?apikey={apiKey}&t={movie.Title}";
            var response = await client.GetStringAsync(url);
            var data = JsonDocument.Parse(response).RootElement;

            if (data.TryGetProperty("Response", out var respProp) && respProp.GetString() == "True")
            {
                movie.PosterUrl = data.GetProperty("Poster").GetString();
                movie.Plot = data.GetProperty("Plot").GetString();
                movie.Genre = data.GetProperty("Genre").GetString();

                if (int.TryParse(data.GetProperty("Year").GetString(), out var yearInt))
                    movie.Year = yearInt;
            }

            movie.Id = Guid.NewGuid();
            movie.CreatedAt = DateTime.UtcNow;

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ---------------- EDIT ----------------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Movie movie)
        {
            if (id != movie.Id) return BadRequest();
            if (!ModelState.IsValid) return View(movie);

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();

                TempData["Toast"] = "Филмот е успешно ажуриран.";
                return RedirectToAction(nameof(Details), new { id = movie.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(e => e.Id == movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // ---------------- DELETE ----------------
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            var vm = new DeleteMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Plot = movie.Plot,
                PosterUrl = movie.PosterUrl
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            TempData["Toast"] = "Филмот е избришан.";
            return RedirectToAction(nameof(Index));
        }
    }
}
