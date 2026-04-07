using System;
using ClassicCars.Data;
using ClassicCars.Dtos;
using ClassicCars.Models;
using ClassicCars.Repositories;
using ClassicCars.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Services
{
    public class WarrantyService:IWarrantyService
    {
        private readonly IWarrantyRepository _repository;

        public WarrantyService(IWarrantyRepository repository)
        {
            _repository = repository;
        }

        public async Task<WarrantyDto?> GetByCarIdAsync(int carId)
        {
            var warranty = await _repository.GetByCarIdAsync(carId);
            if (warranty == null) return null;

            return new WarrantyDto
            {
                CarId = warranty.CarId,
                Provider = warranty.Provider,
                CoverageAmount = warranty.CoverageAmount,
                StartDate = warranty.StartDate,
                EndDate = warranty.EndDate,
                Notes = warranty.Notes
            };
        }

        public async Task AddWarrantyAsync(WarrantyDto dto)
        {
            var warranty = new Warranty
            {
                CarId = dto.CarId,
                Provider = dto.Provider,
                CoverageAmount = dto.CoverageAmount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Notes = dto.Notes,
                CreatedOn = DateTime.UtcNow
            };

            await _repository.AddAsync(warranty);
        }

        public async Task DeleteWarrantyAsync(int carId, string? userId)
        {
            var warranty = await _repository.GetByCarIdAsync(carId);
            if (warranty == null || warranty.Car.UserId != userId)
                return;

            await _repository.DeleteAsync(warranty);
        }
    }
}

