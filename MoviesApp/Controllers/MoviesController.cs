#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _omdbApiKey;

        public MoviesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _httpClient = new HttpClient();
            _omdbApiKey = configuration.GetValue<string>("Omdb:ApiKey") ?? throw new InvalidOperationException("OMDB API Key not found.");
        }

        // ---------------- INDEX ----------------
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchTerm, string? genreFilter)
        {
            var moviesQuery = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                moviesQuery = moviesQuery.Where(m =>
                    !string.IsNullOrEmpty(m.Title) &&
                    EF.Functions.Like(m.Title, $"%{searchTerm}%"));
            }

            if (!string.IsNullOrWhiteSpace(genreFilter) && genreFilter != "Сите жанрови")
            {
                moviesQuery = moviesQuery.Where(m =>
                    !string.IsNullOrEmpty(m.Genre) &&
                    EF.Functions.Like(m.Genre, $"%{genreFilter}%"));
            }

            var movies = await moviesQuery.OrderByDescending(m => m.Year).ToListAsync();

            var genres = _context.Movies
                .Select(m => m.Genre ?? "")
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            genres.Insert(0, "Сите жанрови");

            var viewModel = new MovieIndexViewModel
            {
                Movies = movies,
                SearchString = searchTerm,
                GenreFilter = genreFilter,
                AvailableGenres = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(genres)
            };

            return View(viewModel);
        }



        // ---------------- DETAILS ----------------
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null) return NotFound();

            return View(new MovieDetailsViewModel { Movie = movie });
        }

        // ---------------- CREATE ----------------
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string title, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError(string.Empty, "Името на филмот е задолжително.");
                return View();
            }

            string apiUrl = $"http://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={_omdbApiKey}";
            var response = await _httpClient.GetAsync(apiUrl, ct);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Настана грешка при пребарување на филмот.");
                return View();
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            var omdbMovie = JsonSerializer.Deserialize<OmdbMovieResponse>(json);

            if (omdbMovie == null || omdbMovie.Response == "False")
            {
                ModelState.AddModelError(string.Empty, "Филмот не е пронајден.");
                return View();
            }

            var movie = new Movie
            {
                Title = omdbMovie.Title ?? title,
                Year = int.TryParse(omdbMovie.Year, out int year) ? year : (int?)null,
                Genre = omdbMovie.Genre,
                Plot = omdbMovie.Plot,
                PosterUrl = omdbMovie.Poster,
                ImdbId = omdbMovie.ImdbId,
                Note = null,
                Status = WatchStatus.None
            };

            await _context.Movies.AddAsync(movie, ct);
            await _context.SaveChangesAsync(ct);

            TempData["Toast"] = $"Додаден: {movie.Title}";
            return RedirectToAction("Index");
        }

        // ---------------- EDIT ----------------
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null) return NotFound();

            var vm = new EditMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title ?? "",
                Year = movie.Year,
                Genre = movie.Genre ?? "",
                Plot = movie.Plot ?? "",
                PosterUrl = movie.PosterUrl ?? "",
                ImdbId = movie.ImdbId ?? "",
                Note = movie.Note ?? "",
                Status = movie.Status,
                CreatedAt = movie.CreatedAt 
            };


            return View(vm); 

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditMovieViewModel vm, CancellationToken ct)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var movie = await _context.Movies.FindAsync(id);
            if (movie is null) return NotFound();

            movie.Title = vm.Title;
            movie.Year = vm.Year;
            movie.Genre = vm.Genre;
            movie.Plot = vm.Plot;
            movie.PosterUrl = vm.PosterUrl;
            movie.ImdbId = vm.ImdbId;
            movie.Note = vm.Note;
            movie.Status = vm.Status;
            movie.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync(ct);
                TempData["Toast"] = "Сочувани промени.";
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Настана грешка при зачувување на податоците.");
                return View(vm);
            }

            return RedirectToAction("Details", new { id = movie.Id });
        }

        // ---------------- DELETE ----------------
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null) return NotFound();

            var vm = new DeleteMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title ?? "",
                Year = movie.Year,
                Genre = movie.Genre ?? "",
                Plot = movie.Plot ?? "",
                PosterUrl = movie.PosterUrl ?? "",
                ImdbId = movie.ImdbId ?? ""
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is not null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            TempData["Toast"] = "Филмот е избришан.";
            return RedirectToAction("Index");
        }
    }
    
}
