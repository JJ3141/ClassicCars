using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
using ClassicCars.ViewModels.Car;

namespace ClassicCars.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = null!;

        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(MaxLenghtUsername, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(MinLenghtUsername, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Username { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(MaxEmailLenght, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(MinEmailLenght, ErrorMessage = "{0} must be at least {1} characters.")]
        [EmailAddress(ErrorMessage = "Invalid {0} format.")]
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

        public virtual ICollection<CarDetailsViewModel> Cars { get; set; }
            = new HashSet<CarDetailsViewModel>();

        public virtual CarCreateViewModel NewCar { get; set; } = new CarCreateViewModel();
        public virtual ClassicCars.ViewModels.Car.EditCarViewModel? EditCar { get; set; }
    }
}