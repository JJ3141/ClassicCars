using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.ViewModels.Car
{
    public class EditCarViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Brand")]
        [MaxLength(CarBrandMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(CarBrandMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? Brand { get; set; }

        [Display(Name = "Model")]
        [MaxLength(CarModelMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(CarModelMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? Model { get; set; }

        [Display(Name = "Year")]
        [Range(1900, 2100, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int Year { get; set; }

        [Display(Name = "Engine Type")]
        [MaxLength(EngineTypeMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(EngineTypeMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? EngineType { get; set; }

        [Display(Name = "Horsepower")]
        [Range(1, 5000, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int Horsepower { get; set; }

        [Display(Name = "Condition")]
        [MaxLength(ConditionMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(ConditionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? Condition { get; set; }

        [Display(Name = "Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "{0} must be a positive number.")]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        [MaxLength(DescriptionMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        [MinLength(DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? Description { get; set; }

        [Display(Name = "Transmission Type")]
        [MaxLength(TransmissionMaxLength, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [MinLength(TransmissionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string? Transmission { get; set; }

        [Display(Name = "Car Image")]
        public IFormFile? Image { get; set; }
    }
}