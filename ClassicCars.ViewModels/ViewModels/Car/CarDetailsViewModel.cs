using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
namespace ClassicCars.ViewModels
{
    public class CarDetailsViewModel
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

        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(EngineTypeMaxLength)]
        [MinLength(EngineTypeMinLength)]
        [Display(Name = "Engine Type")]
        public string EngineType { get; set; } = null!;

        [Range(0, 5000, ErrorMessage = "Horsepower must be positive and realistic")]
        [Display(Name = "Horse Power")]
        public int Horsepower { get; set; }

        [Required]
        [MaxLength(ConditionMaxLength)]
        [MinLength(ConditionMinLength)]
        public string Condition { get; set; } = null!;

        [Required]
        [MaxLength(TransmissionMaxLength)]
        [MinLength(TransmissionMinLength)]
        [Display(Name = "Transmission Type")]
        public string Transmission { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Display(Name = "Upload Image")]
        public byte[] ImageData { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public virtual ICollection<ServiceRecordViewModel> ServiceRecords { get; set; }
            = new HashSet<ServiceRecordViewModel>();
    }
}
