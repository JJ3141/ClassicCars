using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassicCars.ViewModels.Car
{
    public class CarCollectionViewModel
    {
        public virtual ICollection<CarCardViewModel> Cars { get; set; }
            = new HashSet<CarCardViewModel>();

        [Range(1, int.MaxValue, ErrorMessage = "Current page must be at least 1")]
        public int CurrentPage { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Total pages must be at least 1")]
        public int TotalPages { get; set; } = 1;

        [StringLength(50, ErrorMessage = "Search term cannot be longer than 50 characters")]
        public string? SearchTerm { get; set; }

        public bool SortByPriceAscending { get; set; } = true;
    }
}
