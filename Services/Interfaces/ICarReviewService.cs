using ClassicCars.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassicCars.Services.Interfaces
{
    public interface ICarReviewService
    {
        Task<IEnumerable<CarReviewViewModel>> GetReviewsForCarAsync(int carId);
        Task AddReviewAsync(int carId, string userId, CarReviewViewModel review);
        Task DeleteReviewAsync(int reviewId, string userId);
    }
}