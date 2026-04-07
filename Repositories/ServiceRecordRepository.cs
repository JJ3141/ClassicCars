using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassicCars.Repositories
{
    public class ServiceRecordRepository : IServiceRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ServiceRecord record)
        {
            await _context.ServiceRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ServiceRecord record)
        {
            _context.ServiceRecords.Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceRecord?> GetByIdAsync(int id)
        {
            return await _context.ServiceRecords.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceRecord>> GetForCarAsync(int carId)
        {
            return await _context.ServiceRecords.Where(r => r.CarId == carId).ToListAsync();
        }
    }
}