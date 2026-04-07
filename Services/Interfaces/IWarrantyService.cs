using System;
using ClassicCars.Dtos;
using ClassicCars.Models;

namespace ClassicCars.Services.Interfaces
{
	public interface IWarrantyService
	{
        Task<WarrantyDto?> GetByCarIdAsync(int carId);
        Task AddWarrantyAsync(WarrantyDto dto);
        Task DeleteWarrantyAsync(int carId, string? userId);
    }
}

