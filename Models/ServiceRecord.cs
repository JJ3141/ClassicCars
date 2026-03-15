
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using static ClassicCars.EntityValidations;
namespace ClassicCars.Models
{
	public class ServiceRecord
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ServiceDate { get; set; }

        [Required]
        [MaxLength(ServiceDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0, 1000000, ErrorMessage = "Mileage must be between 0 and 1,000,000")]
        public double Mileage { get; set; }

        [Required]
        public ServiceType ServiceType { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }

        [ValidateNever]
        public Car Car { get; set; } = null!;



    }
}

