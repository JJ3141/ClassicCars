namespace ClassicCars.Models
{
    using System.ComponentModel.DataAnnotations;
    using static ClassicCars.EntityValidations.User;

    public class Login
	{
  
        public string UsernameOrEmail { get; set; }
        //[MaxLength(MaxLenghtUsername)
        [Required]
        public string Password { get; set; } = null!;
    }
}

