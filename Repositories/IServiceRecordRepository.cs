using ClassicCars.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassicCars.Repositories
{
    public interface IServiceRecordRepository
    {
        Task<ServiceRecord?> GetByIdAsync(int id);
        Task AddAsync(ServiceRecord record);
        Task DeleteAsync(ServiceRecord record);
        Task<IEnumerable<ServiceRecord>> GetForCarAsync(int carId);
    }
}