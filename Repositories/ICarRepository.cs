using ClassicCars.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassicCars.Repositories
{
    public interface ICarRepository
    {
        Task<Car?> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAllAsync();
        Task AddAsync(Car car);
        Task DeleteAsync(Car car);
    }
}