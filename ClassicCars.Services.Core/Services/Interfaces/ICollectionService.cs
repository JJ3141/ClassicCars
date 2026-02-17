using System;
using ClassicCars.ViewModels.Car;

namespace ClassicCars.Services.Interfaces
{
	public interface ICollectionService
	{
        Task<CarCollectionViewModel> GetPagedCarsAsync(int page);
    }
}

