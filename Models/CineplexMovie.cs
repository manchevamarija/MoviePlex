#nullable enable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MoviesApp.Models
{
    public class CineplexxMovie
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("posterImage")]
        public string PosterImage { get; set; } = string.Empty;

        [JsonPropertyName("descriptionCalculated")]
        public string DescriptionCalculated { get; set; } = string.Empty;

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; } = new();

        [JsonPropertyName("rating")]
        public string Rating { get; set; } = string.Empty;

        [JsonPropertyName("actors")]
        public List<string>? Actors { get; set; }

        [JsonPropertyName("trailers")]
        public List<CineplexxTrailer>? Trailers { get; set; }
    }

    public class CineplexxTrailer
    {
        [JsonPropertyName("universalPlayerUrl")]
        public string UniversalPlayerUrl { get; set; } = string.Empty;
    }
}