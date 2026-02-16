using System;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
namespace ClassicCars.ViewModels.Car
{
	public class CarCardViewModel
	{
        public int Id { get; set; }

        [Required]
        [MaxLength(CarBrandMaxLength)]
        [MinLength(CarBrandMinLength)]
        public string Brand { get; set; } = null!;

        [Required]
        [MaxLength(CarModelMaxLength)]
        [MinLength(CarModelMinLength)]
        public string Model { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string? Description { get; set; }

        public byte[]? ImageData { get; set; }
    }
}

