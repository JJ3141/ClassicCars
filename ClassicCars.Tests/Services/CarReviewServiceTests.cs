using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services;
using ClassicCars.Tests.Helpers;
using ClassicCars.ViewModels;

namespace ClassicCars.Tests.Services
{
    public class CarReviewServiceTests
    {
        private ApplicationDbContext CreateContext() => DbContextHelper.CreateInMemoryContext();

        [Fact]
        public async Task GetReviewsForCarAsync_ReturnsReviews_WhenReviewsExist()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "TestUser", Email = "t@t.com", FirstName = "Test", LastName = "User" };
            context.Users.Add(user);

            var car = new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" };
            context.Cars.Add(car);

            context.CarReviews.AddRange(
                new CarReview { Id = 1, CarId = 1, UserId = "u1", Rating = 5, Comment = "Excellent", CreatedOn = new DateTime(2024, 1, 1) },
                new CarReview { Id = 2, CarId = 1, UserId = "u1", Rating = 3, Comment = "Good", CreatedOn = new DateTime(2024, 6, 1) }
            );
            await context.SaveChangesAsync();

            var service = new CarReviewService(context);

            var result = (await service.GetReviewsForCarAsync(1)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Good", result[0].Comment);
            Assert.Equal("Excellent", result[1].Comment);
        }

        [Fact]
        public async Task GetReviewsForCarAsync_ReturnsEmpty_WhenNoReviewsExist()
        {
            using var context = CreateContext();
            var service = new CarReviewService(context);

            var result = await service.GetReviewsForCarAsync(999);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReviewsForCarAsync_ReturnsReviewsOrderedByDateDescending()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "User1", Email = "u@t.com", FirstName = "F", LastName = "L" };
            context.Users.Add(user);

            var car = new Car { Id = 1, Brand = "BMW", Model = "M3", Year = 2020, Price = 70000m, Horsepower = 473, UserId = "u1" };
            context.Cars.Add(car);

            context.CarReviews.AddRange(
                new CarReview { Id = 1, CarId = 1, UserId = "u1", Rating = 3, Comment = "Old review", CreatedOn = new DateTime(2023, 1, 1) },
                new CarReview { Id = 2, CarId = 1, UserId = "u1", Rating = 5, Comment = "New review", CreatedOn = new DateTime(2024, 6, 1) },
                new CarReview { Id = 3, CarId = 1, UserId = "u1", Rating = 4, Comment = "Middle review", CreatedOn = new DateTime(2024, 3, 1) }
            );
            await context.SaveChangesAsync();

            var service = new CarReviewService(context);

            var result = (await service.GetReviewsForCarAsync(1)).ToList();

            Assert.Equal("New review", result[0].Comment);
            Assert.Equal("Middle review", result[1].Comment);
            Assert.Equal("Old review", result[2].Comment);
        }

        [Fact]
        public async Task AddReviewAsync_AddsReviewSuccessfully()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "TestUser", Email = "t@t.com", FirstName = "Test", LastName = "User" };
            context.Users.Add(user);

            var car = new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" };
            context.Cars.Add(car);
            await context.SaveChangesAsync();

            var service = new CarReviewService(context);

            var reviewVm = new CarReviewViewModel
            {
                CarId = 1,
                Rating = 4,
                Comment = "Nice classic car"
            };

            await service.AddReviewAsync(1, "u1", reviewVm);

            var saved = context.CarReviews.First();
            Assert.Equal(1, saved.CarId);
            Assert.Equal("u1", saved.UserId);
            Assert.Equal(4, saved.Rating);
            Assert.Equal("Nice classic car", saved.Comment);
        }

        [Fact]
        public async Task DeleteReviewAsync_DeletesReview_WhenOwnedByUser()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "TestUser", Email = "t@t.com", FirstName = "Test", LastName = "User" };
            context.Users.Add(user);

            var car = new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" };
            context.Cars.Add(car);

            context.CarReviews.Add(new CarReview { Id = 1, CarId = 1, UserId = "u1", Rating = 5, Comment = "Great", CreatedOn = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var service = new CarReviewService(context);

            await service.DeleteReviewAsync(1, "u1");

            Assert.Empty(context.CarReviews);
        }

        [Fact]
        public async Task DeleteReviewAsync_DoesNotDelete_WhenNotOwnedByUser()
        {
            using var context = CreateContext();

            var user = new ApplicationUser { Id = "u1", UserName = "TestUser", Email = "t@t.com", FirstName = "Test", LastName = "User" };
            context.Users.Add(user);

            var car = new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" };
            context.Cars.Add(car);

            context.CarReviews.Add(new CarReview { Id = 1, CarId = 1, UserId = "u1", Rating = 5, Comment = "Great", CreatedOn = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var service = new CarReviewService(context);

            await service.DeleteReviewAsync(1, "wrong-user");

            Assert.Single(context.CarReviews);
        }

        [Fact]
        public async Task DeleteReviewAsync_DoesNothing_WhenReviewDoesNotExist()
        {
            using var context = CreateContext();
            var service = new CarReviewService(context);

            await service.DeleteReviewAsync(999, "u1");

            Assert.Empty(context.CarReviews);
        }
    }
}
