using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Tests.Helpers;
using ClassicCars.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Tests.Services
{
    public class CarServiceTests
    {
        private ApplicationDbContext CreateContext() => DbContextHelper.CreateInMemoryContext();

        [Fact]
        public async Task GetDetailsAsync_ReturnsCarDetails_WhenCarExists()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "TestUser", Email = "test@test.com", FirstName = "Test", LastName = "User" };
            context.Users.Add(user);

            var car = new Car
            {
                Id = 1,
                Brand = "Ford",
                Model = "Mustang",
                Year = 1967,
                Price = 50000m,
                Horsepower = 320,
                UserId = "u1"
            };
            context.Cars.Add(car);

            var review = new CarReview
            {
                Id = 1,
                CarId = 1,
                UserId = "u1",
                Rating = 5,
                Comment = "Great car!",
                CreatedOn = DateTime.UtcNow
            };
            context.CarReviews.Add(review);

            var record = new ServiceRecord
            {
                Id = 1,
                CarId = 1,
                ServiceDate = DateTime.UtcNow,
                Description = "Oil change",
                Mileage = 10000,
                ServiceType = ServiceType.Maintenance
            };
            context.ServiceRecords.Add(record);
            await context.SaveChangesAsync();

            var service = new CarService(context);

            var result = await service.GetDetailsAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Ford", result.Brand);
            Assert.Equal("Mustang", result.Model);
            Assert.Equal(1967, result.Year);
            Assert.Equal(50000m, result.Price);
            Assert.Single(result.Reviews);
            Assert.Single(result.ServiceRecords);
        }

        [Fact]
        public async Task GetDetailsAsync_ReturnsNull_WhenCarDoesNotExist()
        {
            using var context = CreateContext();
            var service = new CarService(context);

            var result = await service.GetDetailsAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetDetailsAsync_MapsReviewFields_Correctly()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "Reviewer", Email = "r@test.com", FirstName = "Rev", LastName = "Iew" };
            context.Users.Add(user);

            var car = new Car { Id = 1, Brand = "BMW", Model = "M3", Year = 2020, Price = 70000m, Horsepower = 473, UserId = "u1" };
            context.Cars.Add(car);

            var review = new CarReview { Id = 1, CarId = 1, UserId = "u1", Rating = 4, Comment = "Nice!", CreatedOn = new DateTime(2024, 1, 1) };
            context.CarReviews.Add(review);
            await context.SaveChangesAsync();

            var service = new CarService(context);
            var result = await service.GetDetailsAsync(1);

            Assert.NotNull(result);
            var r = result.Reviews.First();
            Assert.Equal(4, r.Rating);
            Assert.Equal("Nice!", r.Comment);
            Assert.Equal("Reviewer", r.UserName);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCars()
        {
            using var context = CreateContext();

            context.Cars.AddRange(
                new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" },
                new Car { Id = 2, Brand = "Chevrolet", Model = "Camaro", Year = 1969, Price = 55000m, Horsepower = 350, UserId = "u1" }
            );
            await context.SaveChangesAsync();

            var service = new CarService(context);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenNoCars()
        {
            using var context = CreateContext();
            var service = new CarService(context);

            var result = await service.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenCarExistsAndUserMatches()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var service = new CarService(context);

            var result = await service.DeleteAsync(1, "u1");

            Assert.True(result);
            Assert.Empty(context.Cars);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenCarDoesNotExist()
        {
            using var context = CreateContext();
            var service = new CarService(context);

            var result = await service.DeleteAsync(999, "u1");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotMatch()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var service = new CarService(context);

            var result = await service.DeleteAsync(1, "wrong-user");

            Assert.False(result);
            Assert.Single(context.Cars);
        }
    }
}
