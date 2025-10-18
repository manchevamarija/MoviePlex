#nullable enable

using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class CreateMovieViewModel
    {
        [Required(ErrorMessage = "Името на филмот е задолжително.")]
        public string Title { get; set; } = string.Empty;
    }
}
