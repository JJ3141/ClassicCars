using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services;
using ClassicCars.Tests.Helpers;

namespace ClassicCars.Tests.Services
{
    public class CollectionServiceTests
    {
        private ApplicationDbContext CreateContext() => DbContextHelper.CreateInMemoryContext();

        [Fact]
        public async Task GetPagedCarsAsync_ReturnsFirstPage()
        {
            using var context = CreateContext();

            for (int i = 1; i <= 5; i++)
            {
                context.Cars.Add(new Car
                {
                    Id = i,
                    Brand = $"Brand{i}",
                    Model = $"Model{i}",
                    Year = 1960 + i,
                    Price = 10000m * i,
                    Horsepower = 200 + i,
                    Description = $"Car {i}",
                    UserId = "u1"
                });
            }
            await context.SaveChangesAsync();

            var service = new CollectionService(context);

            var result = await service.GetPagedCarsAsync(1);

            Assert.Equal(5, result.Cars.Count);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task GetPagedCarsAsync_ReturnsEmpty_WhenNoCars()
        {
            using var context = CreateContext();
            var service = new CollectionService(context);

            var result = await service.GetPagedCarsAsync(1);

            Assert.Empty(result.Cars);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(0, result.TotalPages);
        }

        [Fact]
        public async Task GetPagedCarsAsync_ReturnsCorrectPagination_WhenMultiplePages()
        {
            using var context = CreateContext();

            for (int i = 1; i <= 30; i++)
            {
                context.Cars.Add(new Car
                {
                    Id = i,
                    Brand = $"Brand{i}",
                    Model = $"Model{i}",
                    Year = 1960 + i,
                    Price = 10000m * i,
                    Horsepower = 200,
                    UserId = "u1"
                });
            }
            await context.SaveChangesAsync();

            var service = new CollectionService(context);

            var page1 = await service.GetPagedCarsAsync(1);
            var page2 = await service.GetPagedCarsAsync(2);

            Assert.Equal(24, page1.Cars.Count);
            Assert.Equal(2, page1.TotalPages);
            Assert.Equal(1, page1.CurrentPage);

            Assert.Equal(6, page2.Cars.Count);
            Assert.Equal(2, page2.TotalPages);
            Assert.Equal(2, page2.CurrentPage);
        }

        [Fact]
        public async Task GetPagedCarsAsync_MapsFieldsCorrectly()
        {
            using var context = CreateContext();

            context.Cars.Add(new Car
            {
                Id = 1,
                Brand = "Porsche",
                Model = "911",
                Year = 1973,
                Price = 120000m,
                Horsepower = 210,
                Description = "Classic Porsche",
                ImageData = new byte[] { 1, 2, 3 },
                UserId = "u1"
            });
            await context.SaveChangesAsync();

            var service = new CollectionService(context);

            var result = await service.GetPagedCarsAsync(1);

            var card = result.Cars.First();
            Assert.Equal(1, card.Id);
            Assert.Equal("Porsche", card.Brand);
            Assert.Equal("911", card.Model);
            Assert.Equal(120000m, card.Price);
            Assert.Equal("Classic Porsche", card.Description);
            Assert.Equal(new byte[] { 1, 2, 3 }, card.ImageData);
        }
    }
}
