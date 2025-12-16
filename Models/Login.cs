namespace ClassicCars.Models
{
    using System.ComponentModel.DataAnnotations;
    using static ClassicCars.EntityValidations.User;

    public class Login
	{
        [Required]
        //[MaxLength(MaxLenghtUsername)]
        public string Username { get; set; } = null!;
        [Required]
  
        public string Password { get; set; } = null!;
    }
}

