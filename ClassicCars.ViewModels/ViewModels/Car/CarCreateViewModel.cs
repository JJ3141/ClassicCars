using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
using Microsoft.AspNetCore.Http;
namespace ClassicCars.ViewModels.Car
{
    public class CarCreateViewModel
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

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; }

        [Required]
        [MaxLength(EngineTypeMaxLength)]
        [MinLength(EngineTypeMinLength)]
        [Display(Name = "Engine Type")]
        public string EngineType { get; set; } = null!;

        public int Horsepower { get; set; }

        [Required]
        [MaxLength(ConditionMaxLength)]
        [MinLength(ConditionMinLength)]
        public string Condition { get; set; } = null!;

        [Required]
        [MaxLength(TransmissionMaxLength)]
        [MinLength(TransmissionMinLength)]
        public string Transmission { get; set; } = null!;

        public decimal Price { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        public IFormFile? Image { get; set; }

        [Display(Name = "Upload Image")]
        public byte[] ImageData { get; set; } = null!;

    }
}
