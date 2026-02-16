using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
using ClassicCars.ViewModels.Car;
namespace ClassicCars.ViewModels
{
    public class ProfileViewModel
    {
        
        public string Id { get; set; }

        [Required]
        [MaxLength(MaxLenghtUsername)]
        [MinLength(MinLenghtUsername)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(MaxEmailLenght)]
        [MinLength(MinEmailLenght)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(MaxLenghtName)]
        [MinLength(MinLenghtName)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(MaxLenghtName)]
        [MinLength(MinLenghtName)]
        public string LastName { get; set; } = null!;

       
        public virtual ICollection<CarDetailsViewModel> Cars { get; set; }
               = new HashSet<CarDetailsViewModel>();

        public virtual CarCreateViewModel NewCar { get; set; } = new CarCreateViewModel();

    }
}
