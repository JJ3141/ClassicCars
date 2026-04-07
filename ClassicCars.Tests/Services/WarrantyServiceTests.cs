using ClassicCars.Dtos;
using ClassicCars.Models;
using ClassicCars.Repositories;
using ClassicCars.Services;
using Moq;

namespace ClassicCars.Tests.Services
{
    public class WarrantyServiceTests
    {
        private readonly Mock<IWarrantyRepository> _repoMock;
        private readonly WarrantyService _service;

        public WarrantyServiceTests()
        {
            _repoMock = new Mock<IWarrantyRepository>();
            _service = new WarrantyService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByCarIdAsync_ReturnsDto_WhenWarrantyExists()
        {
            var warranty = new Warranty
            {
                Id = 1,
                CarId = 10,
                Provider = "AutoGuard",
                CoverageAmount = 5000m,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2025, 1, 1),
                Notes = "Full coverage"
            };

            _repoMock.Setup(r => r.GetByCarIdAsync(10)).ReturnsAsync(warranty);

            var result = await _service.GetByCarIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(10, result.CarId);
            Assert.Equal("AutoGuard", result.Provider);
            Assert.Equal(5000m, result.CoverageAmount);
            Assert.Equal(new DateTime(2024, 1, 1), result.StartDate);
            Assert.Equal(new DateTime(2025, 1, 1), result.EndDate);
            Assert.Equal("Full coverage", result.Notes);
        }

        [Fact]
        public async Task GetByCarIdAsync_ReturnsNull_WhenWarrantyDoesNotExist()
        {
            _repoMock.Setup(r => r.GetByCarIdAsync(999)).ReturnsAsync((Warranty?)null);

            var result = await _service.GetByCarIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddWarrantyAsync_CallsRepositoryWithMappedEntity()
        {
            var dto = new WarrantyDto
            {
                CarId = 5,
                Provider = "WarrantyCo",
                CoverageAmount = 3000m,
                StartDate = new DateTime(2024, 3, 1),
                EndDate = new DateTime(2026, 3, 1),
                Notes = "Engine only"
            };

            Warranty? capturedWarranty = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Warranty>()))
                .Callback<Warranty>(w => capturedWarranty = w)
                .Returns(Task.CompletedTask);

            await _service.AddWarrantyAsync(dto);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Warranty>()), Times.Once);
            Assert.NotNull(capturedWarranty);
            Assert.Equal(5, capturedWarranty.CarId);
            Assert.Equal("WarrantyCo", capturedWarranty.Provider);
            Assert.Equal(3000m, capturedWarranty.CoverageAmount);
            Assert.Equal(new DateTime(2024, 3, 1), capturedWarranty.StartDate);
            Assert.Equal(new DateTime(2026, 3, 1), capturedWarranty.EndDate);
            Assert.Equal("Engine only", capturedWarranty.Notes);
        }

        [Fact]
        public async Task DeleteWarrantyAsync_DeletesWarranty_WhenUserOwnsTheCar()
        {
            var warranty = new Warranty
            {
                Id = 1,
                CarId = 10,
                Provider = "AutoGuard",
                Car = new Car { Id = 10, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" }
            };

            _repoMock.Setup(r => r.GetByCarIdAsync(10)).ReturnsAsync(warranty);
            _repoMock.Setup(r => r.DeleteAsync(warranty)).Returns(Task.CompletedTask);

            await _service.DeleteWarrantyAsync(10, "u1");

            _repoMock.Verify(r => r.DeleteAsync(warranty), Times.Once);
        }

        [Fact]
        public async Task DeleteWarrantyAsync_DoesNotDelete_WhenUserDoesNotOwnCar()
        {
            var warranty = new Warranty
            {
                Id = 1,
                CarId = 10,
                Provider = "AutoGuard",
                Car = new Car { Id = 10, Brand = "Ford", Model = "Mustang", Year = 1967, Price = 50000m, Horsepower = 320, UserId = "u1" }
            };

            _repoMock.Setup(r => r.GetByCarIdAsync(10)).ReturnsAsync(warranty);

            await _service.DeleteWarrantyAsync(10, "wrong-user");

            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Warranty>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWarrantyAsync_DoesNothing_WhenWarrantyDoesNotExist()
        {
            _repoMock.Setup(r => r.GetByCarIdAsync(999)).ReturnsAsync((Warranty?)null);

            await _service.DeleteWarrantyAsync(999, "u1");

            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Warranty>()), Times.Never);
        }
    }
}
