using ClassicCars.Models;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;
namespace ClassicCars.ViewModels
{
    public class ServiceRecordViewModel
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; } 

        [Required]
        public DateTime ServiceDate { get; set; }

        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        public double Mileage { get; set; }

        public ServiceType ServiceType { get; set; }
    }
}
