using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Services
{
    public class CarReviewService : ICarReviewService
    {
        private readonly ApplicationDbContext _context;

        public CarReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarReviewViewModel>> GetReviewsForCarAsync(int carId)
        {
            // If DbSet not configured, return empty list
            if (_context.Set<CarReview>() == null)
                return Enumerable.Empty<CarReviewViewModel>();

            return await _context.CarReviews
                .Include(r => r.User)
                .Where(r => r.CarId == carId)
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new CarReviewViewModel
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    UserName = r.User != null ? r.User.UserName : string.Empty,
                    CreatedOn = r.CreatedOn
                })
                .ToListAsync();
        }

        public async Task AddReviewAsync(int carId, string userId, CarReviewViewModel review)
        {
            var entity = new CarReview
            {
                CarId = carId,
                UserId = userId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedOn = DateTime.UtcNow
            };

            _context.CarReviews.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId, string userId)
        {
            var review = await _context.CarReviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);
            if (review != null)
            {
                _context.CarReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}