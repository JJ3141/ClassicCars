using ClassicCars.Models;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.ViewModels
{
    public class ServiceRecordViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Car")]
        [Required(ErrorMessage = "{0} is required.")]
        public int CarId { get; set; }

        [Display(Name = "Service Date")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(DescriptionMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        [MinLength(DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Description { get; set; } = null!;

        [Display(Name = "Mileage (km)")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be a positive number.")]
        public double Mileage { get; set; }

        [Display(Name = "Service Type")]
        [Required(ErrorMessage = "{0} is required.")]
        public ServiceType ServiceType { get; set; }
    }
}