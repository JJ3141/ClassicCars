using ClassicCars.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ClassicCars.EntityValidations;

namespace ClassicCars.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(CarBrandMaxLength)]
        public string Brand { get; set; } = null!;

        [Required, MaxLength(CarModelMaxLength)]
        public string Model { get; set; } = null!;

        [Range(1900, 2100)]
        public int Year { get; set; }

        [MaxLength(EngineTypeMaxLength)]
        public string? EngineType { get; set; }

        [Required]
        [Range(1, 10000)]
        public int Horsepower { get; set; }

        [MaxLength(ConditionMaxLength)]
        public string? Condition { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public byte[]? ImageData { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? Image { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [MaxLength(TransmissionMaxLength)]
        public string? Transmission { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public virtual ICollection<ServiceRecord> ServiceRecord { get; set; }
            = new HashSet<ServiceRecord>();
    }
}
