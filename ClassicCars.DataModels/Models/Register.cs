using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

public class Register
{
    [Required]
    [MaxLength(MaxLenghtUsername, ErrorMessage = "Username is too long")]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(MaxPasswordLength, ErrorMessage = "Password is too long")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(MaxEmailLenght, ErrorMessage = "Email is too long")]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(MaxLenghtName, ErrorMessage = "First name is too long")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(MaxLenghtName, ErrorMessage = "Last name is too long")]
    public string LastName { get; set; } = null!;
}
