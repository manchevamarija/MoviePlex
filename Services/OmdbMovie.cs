#nullable enable

using System.Text.Json.Serialization;

namespace MoviesApp.Services
{
    public class OmdbMovie
    {
        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Year")]
        public string? Year { get; set; }

        [JsonPropertyName("Genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("Plot")]
        public string? Plot { get; set; }

        [JsonPropertyName("Poster")]
        public string? PosterUrl { get; set; }
        
        [JsonPropertyName("imdbID")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("imdbRating")]
        public string? ImdbRating { get; set; }
    }
}