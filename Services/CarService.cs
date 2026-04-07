using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using Microsoft.EntityFrameworkCore;
using ClassicCars;
public class CarService : ICarService
{
    private readonly ApplicationDbContext _context;

    public CarService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CarDetailsViewModel?> GetDetailsAsync(int id)
    {
        var car = await _context.Cars
            .Include(c => c.Reviews)
                .ThenInclude(r => r.User)
            .Include(c => c.ServiceRecord)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
            return null;

        return new CarDetailsViewModel
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price,
            ImageData = car.ImageData,
            UserId = car.UserId,

    
            Reviews = car.Reviews
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new CarReviewViewModel
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    UserName = r.User != null ? r.User.UserName : "Unknown",
                    CreatedOn = r.CreatedOn
                }).ToList()
            ,
            ServiceRecords = car.ServiceRecord
                .OrderByDescending(s => s.ServiceDate)
                .Select(s => new ServiceRecordViewModel
                {
                    Id = s.Id,
                    CarId = s.CarId,
                    ServiceDate = s.ServiceDate,
                    Description = s.Description,
                    Mileage = s.Mileage,
                    ServiceType = s.ServiceType
                }).ToList()
        };
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        return await _context.Cars.ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        if (car == null)
            return false;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }
    
}