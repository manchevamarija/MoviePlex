#nullable enable
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MoviesApp.Models;

namespace MoviesApp.Services
{
    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey = "3c0096ac"; 

        public OmdbService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://www.omdbapi.com/"); 
        }

        public async Task<Movie?> GetByTitleAsync(string title, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            var url = $"?t={Uri.EscapeDataString(title)}&apikey={_apiKey}";
            var response = await _http.GetStringAsync(url, ct);

            return JsonSerializer.Deserialize<Movie?>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}