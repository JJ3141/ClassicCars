using System;
using ClassicCars.Data;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels.Car;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ApplicationDbContext _context;

        public CollectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CarCollectionViewModel> GetPagedCarsAsync(int page)
        {
            int carsPerPage = 24;

            var query = _context.Cars.AsQueryable();

            int totalCars = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCars / (double)carsPerPage);

            var cars = await query
                .Skip((page - 1) * carsPerPage)
                .Take(carsPerPage)
                .Select(c => new CarCardViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Price = c.Price,
                    Description = c.Description,
                    ImageData = c.ImageData
                })
                .ToListAsync();

            return new CarCollectionViewModel
            {
                Cars = cars,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }
    }

}

