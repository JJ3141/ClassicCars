using System;
using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Repositories
{
    public class WarrantyRepository:IWarrantyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WarrantyRepository> _logger;

        public WarrantyRepository(ApplicationDbContext context, ILogger<WarrantyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Warranty?> GetByCarIdAsync(int carId)
        {
            return await _context.Warranties.Include(w => w.Car)
                                            .FirstOrDefaultAsync(w => w.CarId == carId);
        }

        public async Task AddAsync(Warranty warranty)
        {
            try
            {
                _logger.LogDebug("Adding warranty for CarId {CarId}", warranty.CarId);
                await _context.Warranties.AddAsync(warranty);
                var changes = await _context.SaveChangesAsync();
                _logger.LogInformation("Saved warranty for CarId {CarId}, changes: {Changes}", warranty.CarId, changes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving warranty for CarId {CarId}", warranty.CarId);
                throw;
            }
        }

        public async Task DeleteAsync(Warranty warranty)
        {
            _context.Warranties.Remove(warranty);
            await _context.SaveChangesAsync();
        }
    }
}

