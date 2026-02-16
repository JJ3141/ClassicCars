using System.Security.Claims;
using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using ClassicCars.ViewModels.Car;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Services
{
    public class ProfileService : IProfileService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
 


        public async Task<ProfileViewModel?> GetProfileAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var cars = await _context.Cars
                .Where(c => c.UserId == userId)
                .Select(c => new CarDetailsViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    EngineType = c.EngineType,
                    Horsepower = c.Horsepower,
                    Condition = c.Condition,
                    Transmission = c.Transmission,
                    Description = c.Description,
                    ImageData = c.ImageData,
                    UserId = c.UserId,
                    ServiceRecords = c.ServiceRecord
                        .Select(s => new ServiceRecordViewModel
                        {
                            ServiceDate = s.ServiceDate,
                            Mileage = s.Mileage,
                            Description = s.Description,
                            ServiceType = s.ServiceType
                        }).ToList()
                })
                .ToListAsync();

            return new ProfileViewModel
            {
                Username = user.UserName!,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Cars = cars
            };
        }

       
        public async Task<Car?> GetCarByIdAsync(int carId, string userId)
        {
            return await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == carId && c.UserId == userId);
        }
        public async Task<bool> DeleteCarAsync(int carId, string userId)
        {
            var car = await _context.Cars.FindAsync(carId);

            if (car == null)
            {
                return false; 
            }

            if (car.UserId != userId)
            {
                throw new UnauthorizedAccessException("Cannot delete a car that doesn't belong to you");
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> AddCarAsync(CarCreateViewModel car, ClaimsPrincipal userPrincipal, bool isModelStateValid)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
                return "Login";

            if (car.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    car.Image.CopyTo(memoryStream);
                    car.ImageData = memoryStream.ToArray();
                }
            }

            if (!isModelStateValid)
            {
                var newCar = new Car
                {
                    Brand = car.Brand,
                    Model = car.Model,
                    Year = car.Year,
                    EngineType = car.EngineType,
                    Horsepower = car.Horsepower,
                    Condition = car.Condition,
                    Transmission = car.Transmission,
                    Price = car.Price,
                    Description = car.Description,
                    ImageData = car.ImageData,
                    UserId = user.Id
                };

                _context.Cars.Add(newCar);
                await _context.SaveChangesAsync();

                return "Index";
            }

            return "Profile";
        }

        public async Task EditCarAsync(EditCarViewModel car)
        {
            var existingCar = await _context.Cars.FindAsync(car.Id);
            //Console.WriteLine(car.Id);
            if (existingCar == null)
                return;

            existingCar.Brand = car.Brand;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.EngineType = car.EngineType;
            existingCar.Horsepower = car.Horsepower;
            existingCar.Condition = car.Condition;
            existingCar.Price = car.Price;
            existingCar.Description = car.Description;
            existingCar.Transmission = car.Transmission;

            if (car.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    car.Image.CopyTo(memoryStream);
                    existingCar.ImageData = memoryStream.ToArray();
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}

