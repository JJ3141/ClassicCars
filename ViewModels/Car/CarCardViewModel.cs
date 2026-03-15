using System;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.ViewModels.Car
{
    public class CarCardViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Car Brand")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(CarBrandMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(CarBrandMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Brand { get; set; } = null!;

        [Display(Name = "Car Model")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(CarModelMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(CarModelMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Model { get; set; } = null!;

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "{0} must be a positive number.")]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        [MaxLength(DescriptionMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        [MinLength(DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? Description { get; set; }

        [Display(Name = "Car Image")]
        public byte[]? ImageData { get; set; }
    }
}