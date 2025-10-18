using System.Text.Json.Serialization;

namespace MoviesApp.Models
{
    public class CineplexxShowtime
    {
        [JsonPropertyName("cinemaName")]
        public string CinemaName { get; set; } = string.Empty;

        [JsonPropertyName("startAt")]
        public string StartAt { get; set; } = string.Empty;
    }
}