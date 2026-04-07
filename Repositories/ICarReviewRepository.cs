using ClassicCars.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassicCars.Repositories
{
    public interface ICarReviewRepository
    {
        Task<CarReview?> GetByIdAsync(int id);
        Task<IEnumerable<CarReview>> GetForCarAsync(int carId);
        Task AddAsync(CarReview review);
        Task DeleteAsync(CarReview review);
    }
}