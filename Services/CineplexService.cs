#nullable enable
using MoviesApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoviesApp.Services
{
    public class CineplexxService
    {
        private readonly HttpClient _httpClient;

        public CineplexxService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      
        public async Task<List<CineplexxMovie>> GetMoviesAsync(DateTime date)
        {
            var url = $"https://app.cineplexx.mk/api/v2/movies?date={date:yyyy-MM-dd}&location=all";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var movies = JsonSerializer.Deserialize<List<CineplexxMovie>>(json, options);

            return movies ?? new List<CineplexxMovie>();
        }

        
        public async Task<List<CineplexxMovie>> GetUpcomingMoviesAsync()
        {
            var url = "https://app.cineplexx.mk/api/v2/movies/coming-soon?location=all";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var movies = JsonSerializer.Deserialize<List<CineplexxMovie>>(json, options);

            return movies ?? new List<CineplexxMovie>();
        }
    }
}