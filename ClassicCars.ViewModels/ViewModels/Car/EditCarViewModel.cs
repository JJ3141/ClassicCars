using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.ViewModels.Car
{
    public class EditCarViewModel
    {
        [MaxLength(CarBrandMaxLength, ErrorMessage = "Brand не може да е по-дълъг от {1} символа.")]
        [MinLength(CarBrandMinLength, ErrorMessage = "Brand трябва да е поне {1} символа.")]
        [Display(Name = "Brand")]
        public string? Brand { get; set; }

        [MaxLength(CarModelMaxLength, ErrorMessage = "Model не може да е по-дълъг от {1} символа.")]
        [MinLength(CarModelMinLength, ErrorMessage = "Model трябва да е поне {1} символа.")]
        [Display(Name = "Model")]
        public string? Model { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year трябва да е между 1900 и 2100.")]
        public int Year { get; set; }

        [MaxLength(EngineTypeMaxLength, ErrorMessage = "Engine Type не може да е по-дълъг от {1} символа.")]
        [MinLength(EngineTypeMinLength, ErrorMessage = "Engine Type трябва да е поне {1} символа.")]
        [Display(Name = "Engine Type")]
        public string? EngineType { get; set; }

        [Range(0, 5000, ErrorMessage = "Horsepower must be positive and realistic.")]
        [Display(Name = "Horse Power")]
        public int Horsepower { get; set; }

        [MaxLength(ConditionMaxLength, ErrorMessage = "Condition не може да е по-дълъг от {1} символа.")]
        [MinLength(ConditionMinLength, ErrorMessage = "Condition трябва да е поне {1} символа.")]
        [Display(Name = "Condition")]
        public string? Condition { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price трябва да е положително число.")]
        public decimal Price { get; set; }

        [MaxLength(DescriptionMaxLength, ErrorMessage = "Description не може да е по-дълъг от {1} символа.")]
        [MinLength(DescriptionMinLength, ErrorMessage = "Description трябва да е поне {1} символа.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [MaxLength(TransmissionMaxLength, ErrorMessage = "Transmission Type не може да е по-дълъг от {1} символа.")]
        [MinLength(TransmissionMinLength, ErrorMessage = "Transmission Type трябва да е поне {1} символа.")]
        [Display(Name = "Transmission Type")]
        public string? Transmission { get; set; }

        public IFormFile? Image { get; set; }

        public int Id { get; set; }
    }
}
