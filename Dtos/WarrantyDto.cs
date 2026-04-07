using System;
namespace ClassicCars.Dtos
{
	public class WarrantyDto
	{
        public int CarId { get; set; }
        public string Provider { get; set; } = null!;
        public decimal CoverageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
    }
}

