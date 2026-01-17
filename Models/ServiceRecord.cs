
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassicCars.Models
{
	public class ServiceRecord
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ServiceDate { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public double Mileage { get; set; }

        [Required]
        public ServiceType ServiceType { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }

        [ValidateNever]
        public Car Car { get; set; } = null!;



    }
}

