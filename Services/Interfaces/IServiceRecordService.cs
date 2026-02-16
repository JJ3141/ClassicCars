using System;
using ClassicCars.Models;
using ClassicCars.ViewModels;

namespace ClassicCars.Services.Interfaces
{
    public interface IServiceRecordService
    {
        Task<bool> AddServiceRecordAsync(ServiceRecordViewModel record, string userId);
        Task<ServiceRecordViewModel?> GetByIdAsync(int id, string userId);
        Task<bool> EditAsync(ServiceRecordViewModel updatedRecord, string userId);

    }

}

