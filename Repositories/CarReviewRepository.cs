using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassicCars.Repositories
{
    public class CarReviewRepository : ICarReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public CarReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CarReview review)
        {
            await _context.CarReviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CarReview review)
        {
            _context.CarReviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<CarReview?> GetByIdAsync(int id)
        {
            return await _context.CarReviews.FindAsync(id);
        }

        public async Task<IEnumerable<CarReview>> GetForCarAsync(int carId)
        {
            return await _context.CarReviews.Where(r => r.CarId == carId).ToListAsync();
        }
    }
}