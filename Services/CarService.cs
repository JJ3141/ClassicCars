using System;
using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
            => await _context.Cars.ToListAsync();

        public async Task<CarDetailsViewModel?> GetDetailsAsync(int id)
        {
            var car = await _context.Cars
                .Include(c => c.ServiceRecord)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null) return null;

            return new CarDetailsViewModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Price = car.Price,
                EngineType = car.EngineType,
                Horsepower = car.Horsepower,
                Condition = car.Condition,
                Transmission = car.Transmission,
                Description = car.Description,
                ImageData = car.ImageData,
                UserId = car.UserId,
                ServiceRecords = car.ServiceRecord
                    .OrderByDescending(s => s.ServiceDate)
                    .Select(s => new ServiceRecordViewModel
                    {
                        ServiceDate = s.ServiceDate,
                        Description = s.Description,
                        Mileage = s.Mileage,
                        ServiceType = s.ServiceType
                    })
                    .ToList()
            };
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null || car.UserId != userId)
                return false;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }
        

    }

}

