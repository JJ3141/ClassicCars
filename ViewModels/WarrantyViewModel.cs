using ClassicCars.Models;
using System;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars
{
    public class WarrantyViewModel
    {
        public int CarId { get; set; }

        public Car? Car { get; set; }

        // Indicates whether a warranty record exists for the car
        public bool HasWarranty { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [MinLength(WarrantyProviderMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        [MaxLength(WarrantyProviderMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string Provider { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        [Range(0, 1000000, ErrorMessage = "{0} must be between {1} and {2}")]
        [Display(Name = "Coverage Amount")]
        public decimal CoverageAmount { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [MinLength(WarrantyNotesMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        [MaxLength(WarrantyNotesMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}