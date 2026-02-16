using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
namespace ClassicCars.ViewModels.Car
{
    public class EditCarViewModel
    {
        [MaxLength(CarBrandMaxLength)]
        [MinLength(CarBrandMinLength)]
        [Display(Name = "Brand")]
        public string? Brand { get; set; }

        [MaxLength(CarModelMaxLength)]
        [MinLength(CarModelMinLength)]
        [Display(Name = "Model")]
        public string? Model { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [MaxLength(EngineTypeMaxLength)]
        [MinLength(EngineTypeMinLength)]
        [Display(Name = "Engine Type")]
        public string? EngineType { get; set; }

        [Range(0, 5000, ErrorMessage = "Horsepower must be positive and realistic")]
        [Display(Name = "Horse Power")]
        public int Horsepower { get; set; }

        [MaxLength(ConditionMaxLength)]
        [MinLength(ConditionMinLength)]
        [Display(Name = "Condition")]
        public string? Condition { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [MaxLength(TransmissionMaxLength)]
        [MinLength(TransmissionMinLength)]
        [Display(Name = "Transmission Type")]
        public string? Transmission { get; set; }

        public IFormFile? Image { get; set; }
        public int Id { get; set; }
    }
}
