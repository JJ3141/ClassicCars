using System;
using ClassicCars.Models;

namespace ClassicCars.Repositories
{
	public interface IWarrantyRepository
	{
        Task<Warranty?> GetByCarIdAsync(int carId);
        Task AddAsync(Warranty warranty);
        Task DeleteAsync(Warranty warranty);
    }
}

