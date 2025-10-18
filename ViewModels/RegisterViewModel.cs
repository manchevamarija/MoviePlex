using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Корисничко име")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Е-пошта")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Лозинка")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Лозинките не се совпаѓаат.")]
        [Display(Name = "Потврди лозинка")]
        public string ConfirmPassword { get; set; } = null!;
    }
}