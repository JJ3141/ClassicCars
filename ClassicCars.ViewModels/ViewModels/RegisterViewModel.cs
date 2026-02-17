using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
namespace ClassicCars.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [MaxLength(MaxLenghtUsername)]
        [MinLength(MinLenghtUsername)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(MaxEmailLenght)]
        [MinLength(MinEmailLenght)]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(MaxLenghtName)]
        [MinLength(MinLenghtName)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(MaxLenghtName)]
        [MinLength(MinLenghtName)]
        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength)]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [MinLength(MinPasswordLength)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
