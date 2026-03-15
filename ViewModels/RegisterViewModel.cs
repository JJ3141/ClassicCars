using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(MaxLenghtUsername, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(MinLenghtUsername, ErrorMessage = "{0} must be at least {1} characters.")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "Invalid {0} format.")]
        [MaxLength(MaxEmailLenght, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(MinEmailLenght, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Email { get; set; } = null!;

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(MaxLenghtName, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(MinLenghtName, ErrorMessage = "{0} must be at least {1} characters.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(MaxLenghtName, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(MinLenghtName, ErrorMessage = "{0} must be at least {1} characters.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [MinLength(MinPasswordLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}