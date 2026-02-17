using System.Security.Claims;
using ClassicCars.Models;
using ClassicCars.ViewModels;
using ClassicCars.ViewModels.Car;

namespace ClassicCars.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetProfileAsync(string userId);


        Task<Car?> GetCarByIdAsync(int carId, string userId);
        Task<bool> DeleteCarAsync(int carId, string userId);

        Task EditCarAsync(EditCarViewModel car);
        Task<string> AddCarAsync(CarCreateViewModel car, ClaimsPrincipal userPrincipal, bool isModelStateValid);

    }
}
