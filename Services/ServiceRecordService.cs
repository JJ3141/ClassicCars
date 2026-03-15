using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using Microsoft.EntityFrameworkCore;

public class ServiceRecordService : IServiceRecordService
{
    private readonly ApplicationDbContext _context;

    public ServiceRecordService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddServiceRecordAsync(ServiceRecordViewModel record, string userId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == record.CarId);

        if (car == null || car.UserId != userId)
            return false;

        var newRecord = new ServiceRecord
        {
            CarId = record.CarId,
            ServiceDate = record.ServiceDate,
            Description = record.Description,
            Mileage = record.Mileage,
            ServiceType = record.ServiceType
        };

        _context.ServiceRecords.Add(newRecord);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EditAsync(ServiceRecordViewModel updatedRecord, string userId)
    {
        var record = await _context.ServiceRecords
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == updatedRecord.Id && r.Car.UserId == userId);

        if (record == null)
            return false;

        record.ServiceDate = updatedRecord.ServiceDate;
        record.Description = updatedRecord.Description;
        record.Mileage = updatedRecord.Mileage;
        record.ServiceType = updatedRecord.ServiceType;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ServiceRecordViewModel?> GetByIdAsync(int id, string userId)
    {
        var record = await _context.ServiceRecords
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == id && r.Car.UserId == userId);

        if (record == null)
            return null;

        return new ServiceRecordViewModel
        {
            Id = record.Id,
            ServiceDate = record.ServiceDate,
            Description = record.Description,
            Mileage = record.Mileage,
            ServiceType = record.ServiceType
        };
    }
}
