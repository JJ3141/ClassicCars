using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations.User;
namespace ClassicCars.Models
{
	public class Register
	{
        [Required]
       // [MaxLength(MaxLenghtUsername)]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
    }
}

