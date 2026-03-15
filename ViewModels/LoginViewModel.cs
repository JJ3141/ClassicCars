using System.ComponentModel.DataAnnotations;

namespace ClassicCars.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Username or Email")]
        [Required(ErrorMessage = "{0} is required.")]
        public string UsernameOrEmail { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}