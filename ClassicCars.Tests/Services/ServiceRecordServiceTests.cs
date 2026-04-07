using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Tests.Helpers;
using ClassicCars.ViewModels;

namespace ClassicCars.Tests.Services
{
    public class ServiceRecordServiceTests
    {
        private ApplicationDbContext CreateContext() => DbContextHelper.CreateInMemoryContext();

        [Fact]
        public async Task AddServiceRecordAsync_ReturnsTrue_WhenCarExistsAndUserMatches()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var service = new ServiceRecordService(context);

            var vm = new ServiceRecordViewModel
            {
                CarId = 1,
                ServiceDate = new DateTime(2024, 5, 1),
                Description = "Oil change",
                Mileage = 10000,
                ServiceType = ServiceType.Maintenance
            };

            var result = await service.AddServiceRecordAsync(vm, "u1");

            Assert.True(result);
            Assert.Single(context.ServiceRecords);
            var saved = context.ServiceRecords.First();
            Assert.Equal("Oil change", saved.Description);
            Assert.Equal(10000, saved.Mileage);
            Assert.Equal(ServiceType.Maintenance, saved.ServiceType);
        }

        [Fact]
        public async Task AddServiceRecordAsync_ReturnsFalse_WhenCarDoesNotExist()
        {
            using var context = CreateContext();
            var service = new ServiceRecordService(context);

            var vm = new ServiceRecordViewModel
            {
                CarId = 999,
                ServiceDate = DateTime.UtcNow,
                Description = "Oil change",
                Mileage = 10000,
                ServiceType = ServiceType.Maintenance
            };

            var result = await service.AddServiceRecordAsync(vm, "u1");

            Assert.False(result);
            Assert.Empty(context.ServiceRecords);
        }

        [Fact]
        public async Task AddServiceRecordAsync_ReturnsFalse_WhenUserDoesNotMatch()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var service = new ServiceRecordService(context);

            var vm = new ServiceRecordViewModel
            {
                CarId = 1,
                ServiceDate = DateTime.UtcNow,
                Description = "Oil change",
                Mileage = 10000,
                ServiceType = ServiceType.Maintenance
            };

            var result = await service.AddServiceRecordAsync(vm, "wrong-user");

            Assert.False(result);
            Assert.Empty(context.ServiceRecords);
        }

        [Fact]
        public async Task EditAsync_ReturnsTrue_WhenRecordExistsAndUserMatches()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            context.ServiceRecords.Add(new ServiceRecord
            {
                Id = 1,
                CarId = 1,
                ServiceDate = new DateTime(2024, 1, 1),
                Description = "Old description",
                Mileage = 5000,
                ServiceType = ServiceType.Maintenance
            });
            await context.SaveChangesAsync();

            var service = new ServiceRecordService(context);

            var updated = new ServiceRecordViewModel
            {
                Id = 1,
                CarId = 1,
                ServiceDate = new DateTime(2024, 6, 1),
                Description = "Updated description",
                Mileage = 15000,
                ServiceType = ServiceType.Repair
            };

            var result = await service.EditAsync(updated, "u1");

            Assert.True(result);
            var record = context.ServiceRecords.First();
            Assert.Equal("Updated description", record.Description);
            Assert.Equal(15000, record.Mileage);
            Assert.Equal(ServiceType.Repair, record.ServiceType);
        }

        [Fact]
        public async Task EditAsync_ReturnsFalse_WhenRecordDoesNotExist()
        {
            using var context = CreateContext();
            var service = new ServiceRecordService(context);

            var updated = new ServiceRecordViewModel
            {
                Id = 999,
                ServiceDate = DateTime.UtcNow,
                Description = "Doesn't matter",
                Mileage = 10000,
                ServiceType = ServiceType.Other
            };

            var result = await service.EditAsync(updated, "u1");

            Assert.False(result);
        }

        [Fact]
        public async Task EditAsync_ReturnsFalse_WhenUserDoesNotMatch()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            context.ServiceRecords.Add(new ServiceRecord
            {
                Id = 1,
                CarId = 1,
                ServiceDate = new DateTime(2024, 1, 1),
                Description = "Original",
                Mileage = 5000,
                ServiceType = ServiceType.Maintenance
            });
            await context.SaveChangesAsync();

            var service = new ServiceRecordService(context);

            var updated = new ServiceRecordViewModel
            {
                Id = 1,
                ServiceDate = DateTime.UtcNow,
                Description = "Updated",
                Mileage = 20000,
                ServiceType = ServiceType.Repair
            };

            var result = await service.EditAsync(updated, "wrong-user");

            Assert.False(result);
            var record = context.ServiceRecords.First();
            Assert.Equal("Original", record.Description);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRecord_WhenFoundAndUserMatches()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            context.ServiceRecords.Add(new ServiceRecord
            {
                Id = 1,
                CarId = 1,
                ServiceDate = new DateTime(2024, 3, 15),
                Description = "Brake inspection",
                Mileage = 20000,
                ServiceType = ServiceType.Inspection
            });
            await context.SaveChangesAsync();

            var service = new ServiceRecordService(context);

            var result = await service.GetByIdAsync(1, "u1");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Brake inspection", result.Description);
            Assert.Equal(20000, result.Mileage);
            Assert.Equal(ServiceType.Inspection, result.ServiceType);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenRecordDoesNotExist()
        {
            using var context = CreateContext();
            var service = new ServiceRecordService(context);

            var result = await service.GetByIdAsync(999, "u1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotMatch()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            context.ServiceRecords.Add(new ServiceRecord
            {
                Id = 1,
                CarId = 1,
                ServiceDate = new DateTime(2024, 3, 15),
                Description = "Brake inspection",
                Mileage = 20000,
                ServiceType = ServiceType.Inspection
            });
            await context.SaveChangesAsync();

            var service = new ServiceRecordService(context);

            var result = await service.GetByIdAsync(1, "wrong-user");

            Assert.Null(result);
        }
    }
}
