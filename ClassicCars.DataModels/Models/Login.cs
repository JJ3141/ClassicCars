namespace ClassicCars.Models
{
    using System.ComponentModel.DataAnnotations;
    using static ClassicCars.EntityValidations;

    public class Login
	{
  
        public string UsernameOrEmail { get; set; }

        [Required]
        [MaxLength(MaxPasswordLength)]
        public string Password { get; set; } = null!;
    }
}

