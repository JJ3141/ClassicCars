using System;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.Models
{
    public class Warranty
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }
        public virtual Car Car { get; set; } = null!;

        [Required]
        [MaxLength(WarrantyProviderMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string Provider { get; set; } = null!;

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Display(Name = "Coverage Amount")]
        [Range(0, 1000000, ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal CoverageAmount { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(WarrantyNotesMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string? Notes { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}