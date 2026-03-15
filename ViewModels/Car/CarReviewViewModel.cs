using System;
using System.ComponentModel.DataAnnotations;
using static ClassicCars.EntityValidations;

namespace ClassicCars.ViewModels
{
    public class CarReviewViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }

        [Display(Name = "Rating")]
        [Range(1, 5, ErrorMessage = "{0} must be between {1} and {2}")]
        public int Rating { get; set; } = 5;

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "{0} is required.")]
        [MaxLength(ReviewCommentMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        [MinLength(ReviewCommentMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
        public string Comment { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}