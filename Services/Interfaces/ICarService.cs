using System;
using ClassicCars.Models;
using ClassicCars.ViewModels;

namespace ClassicCars.Services.Interfaces
{
	public interface ICarService
	{
        Task<IEnumerable<Car>> GetAllAsync();
        Task<CarDetailsViewModel?> GetDetailsAsync(int id);
        Task<bool> DeleteAsync(int id, string userId);

    }
}

