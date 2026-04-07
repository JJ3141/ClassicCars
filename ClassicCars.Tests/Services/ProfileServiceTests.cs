using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services;
using ClassicCars.Tests.Helpers;
using ClassicCars.ViewModels.Car;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ClassicCars.Tests.Services
{
    public class ProfileServiceTests
    {
        private ApplicationDbContext CreateContext() => DbContextHelper.CreateInMemoryContext();

        private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        [Fact]
        public async Task GetProfileAsync_ReturnsProfile_WhenUserExists()
        {
            using var context = CreateContext();

            var user = new ApplicationUser
            {
                Id = "u1",
                UserName = "JohnDoe",
                Email = "john@test.com",
                FirstName = "John",
                LastName = "Doe"
            };
            context.Users.Add(user);

            context.Cars.Add(new Car
            {
                Id = 1,
                Brand = "Ford",
                Model = "Mustang",
                Year = 1967,
                Price = 50000m,
                Horsepower = 320,
                EngineType = "V8",
                Condition = "Excellent",
                Transmission = "Manual",
                Description = "Classic muscle car",
                UserId = "u1"
            });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.GetProfileAsync("u1");

            Assert.NotNull(result);
            Assert.Equal("JohnDoe", result.Username);
            Assert.Equal("john@test.com", result.Email);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Single(result.Cars);
            Assert.Equal("Ford", result.Cars.First().Brand);
        }

        [Fact]
        public async Task GetProfileAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.GetProfileAsync("non-existent");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetProfileAsync_ReturnsEmptyCars_WhenUserHasNoCars()
        {
            using var context = CreateContext();
            context.Users.Add(new ApplicationUser { Id = "u1", UserName = "User1", Email = "u@t.com", FirstName = "F", LastName = "L" });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.GetProfileAsync("u1");

            Assert.NotNull(result);
            Assert.Empty(result.Cars);
        }

        [Fact]
        public async Task GetCarByIdAsync_ReturnsCar_WhenFoundAndOwned()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.GetCarByIdAsync(1, "u1");

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Ford", result.Brand);
        }

        [Fact]
        public async Task GetCarByIdAsync_ReturnsNull_WhenCarDoesNotExist()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.GetCarByIdAsync(999, "u1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCarByIdAsync_ReturnsNull_WhenUserDoesNotOwn()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.GetCarByIdAsync(1, "wrong-user");

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCarAsync_ReturnsTrue_WhenCarExistsAndOwned()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.DeleteCarAsync(1, "u1");

            Assert.True(result);
            Assert.Empty(context.Cars);
        }

        [Fact]
        public async Task DeleteCarAsync_ReturnsFalse_WhenCarDoesNotExist()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var result = await service.DeleteCarAsync(999, "u1");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCarAsync_ThrowsUnauthorized_WhenUserDoesNotOwn()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.DeleteCarAsync(1, "wrong-user"));
            Assert.Single(context.Cars);
        }

        [Fact]
        public async Task EditCarAsync_UpdatesCarFields_WhenCarExists()
        {
            using var context = CreateContext();
            context.Cars.Add(new Car
            {
                Id = 1,
                Brand = "Ford",
                Model = "Mustang",
                Year = 1967,
                Price = 50000m,
                Horsepower = 320,
                EngineType = "V8",
                Condition = "Good",
                Description = "Old desc",
                Transmission = "Manual",
                UserId = "u1"
            });
            await context.SaveChangesAsync();

            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var editVm = new EditCarViewModel
            {
                Id = 1,
                Brand = "Ford Updated",
                Model = "Mustang GT",
                Year = 1968,
                Price = 60000m,
                Horsepower = 390,
                EngineType = "V8 Big Block",
                Condition = "Excellent",
                Description = "Updated desc",
                Transmission = "Automatic"
            };

            await service.EditCarAsync(editVm);

            var car = await context.Cars.FindAsync(1);
            Assert.NotNull(car);
            Assert.Equal("Ford Updated", car.Brand);
            Assert.Equal("Mustang GT", car.Model);
            Assert.Equal(1968, car.Year);
            Assert.Equal(60000m, car.Price);
            Assert.Equal(390, car.Horsepower);
            Assert.Equal("V8 Big Block", car.EngineType);
            Assert.Equal("Excellent", car.Condition);
            Assert.Equal("Updated desc", car.Description);
            Assert.Equal("Automatic", car.Transmission);
        }

        [Fact]
        public async Task EditCarAsync_DoesNothing_WhenCarDoesNotExist()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            var service = new ProfileService(mockUserManager.Object, context);

            var editVm = new EditCarViewModel
            {
                Id = 999,
                Brand = "Test",
                Model = "Test",
                Year = 2000,
                Price = 10000m,
                Horsepower = 100
            };

            await service.EditCarAsync(editVm);

            Assert.Empty(context.Cars);
        }

        [Fact]
        public async Task AddCarAsync_ReturnsLogin_WhenUserIsNull()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync((ApplicationUser?)null);

            var service = new ProfileService(mockUserManager.Object, context);

            var carVm = new CarCreateViewModel
            {
                Brand = "Test",
                Model = "Test",
                Year = 2000,
                EngineType = "V4",
                Horsepower = 100,
                Condition = "Good",
                Transmission = "Auto",
                Price = 10000m,
                Description = "Test car"
            };

            var result = await service.AddCarAsync(carVm, new System.Security.Claims.ClaimsPrincipal(), true);

            Assert.Equal("Login", result);
        }

        [Fact]
        public async Task AddCarAsync_ReturnsProfile_WhenModelStateInvalid()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            var user = new ApplicationUser { Id = "u1", UserName = "Test", Email = "t@t.com", FirstName = "F", LastName = "L" };
            mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var service = new ProfileService(mockUserManager.Object, context);

            var carVm = new CarCreateViewModel
            {
                Brand = "Test",
                Model = "Test",
                Year = 2000,
                EngineType = "V4",
                Horsepower = 100,
                Condition = "Good",
                Transmission = "Auto",
                Price = 10000m,
                Description = "Test car"
            };

            var result = await service.AddCarAsync(carVm, new System.Security.Claims.ClaimsPrincipal(), false);

            Assert.Equal("Profile", result);
            Assert.Empty(context.Cars);
        }

        [Fact]
        public async Task AddCarAsync_ReturnsIndex_WhenSuccessful()
        {
            using var context = CreateContext();
            var mockUserManager = CreateMockUserManager();
            var user = new ApplicationUser { Id = "u1", UserName = "Test", Email = "t@t.com", FirstName = "F", LastName = "L" };
            mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var service = new ProfileService(mockUserManager.Object, context);

            var carVm = new CarCreateViewModel
            {
                Brand = "Porsche",
                Model = "911",
                Year = 1973,
                EngineType = "Flat-6",
                Horsepower = 210,
                Condition = "Excellent",
                Transmission = "Manual",
                Price = 120000m,
                Description = "Classic Porsche"
            };

            var result = await service.AddCarAsync(carVm, new System.Security.Claims.ClaimsPrincipal(), true);

            Assert.Equal("Index", result);
            Assert.Single(context.Cars);
            var saved = context.Cars.First();
            Assert.Equal("Porsche", saved.Brand);
            Assert.Equal("911", saved.Model);
            Assert.Equal("u1", saved.UserId);
        }
    }
}
