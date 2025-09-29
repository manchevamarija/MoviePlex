#nullable enable
using System.Text.Json.Serialization;

namespace MoviesApp.Models
{
    public class OmdbMovieResponse
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
        public string? Poster { get; set; }
        [JsonPropertyName("imdbID")]
        public string? ImdbId { get; set; }
        [JsonPropertyName("Response")]
        public string? Response { get; set; }
    }
}