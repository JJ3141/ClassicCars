using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static ClassicCars.EntityValidations;
namespace ClassicCars.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(MaxLenghtName)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(MaxLenghtName)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<CarReview> Reviews { get; set; } = new HashSet<CarReview>();
    }
}
