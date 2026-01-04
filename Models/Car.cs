using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassicCars.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Range(1900, 2100)]
        public int Year { get; set; }

        public string? EngineType { get; set; }

        public int Horsepower { get; set; }

        public string? Condition { get; set; } 

        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set;}

        public string? Transmission { get; set;}

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ValidateNever]
        public User User { get; set; } = null!;

    }
}
