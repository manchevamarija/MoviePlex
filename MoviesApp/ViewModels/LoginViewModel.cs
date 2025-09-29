using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Внесете корисничко име")]
        [Display(Name = "Корисничко име")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Внесете лозинка")]
        [DataType(DataType.Password)]
        [Display(Name = "Лозинка")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запамти ме")]
        public bool RememberMe { get; set; }
    }

}