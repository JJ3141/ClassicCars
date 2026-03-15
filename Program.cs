using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services;
using ClassicCars.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IServiceRecordService, ServiceRecordService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICarReviewService, CarReviewService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
});

var app = builder.Build();

await SeedDataAsync(app);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    try
    {
        await db.Database.MigrateAsync();

        var user1 = await userManager.FindByEmailAsync("miroslava@gmail.com");
        if (user1 == null)
        {
            user1 = new ApplicationUser
            {
                UserName = "user",
                Email = "miroslava@gmail.com",
                FirstName = "Miroslava",
                LastName = "Tsaneva",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user1, "Password123!");
        }

        var user2 = await userManager.FindByEmailAsync("georgi@gmail.com");
        if (user2 == null)
        {
            user2 = new ApplicationUser
            {
                UserName = "user1",
                Email = "georgi@gmail.com",
                FirstName = "Georgi",
                LastName = "Milanov",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user2, "Password123!");
        }

        if (!db.Cars.Any())
        {
            var car1 = new Car
            {
                Brand = "BMW",
                Model = "X5",
                Year = 2022,
                Horsepower = 300,
                Price = 50000,
                UserId = user1.Id,
                ImageData = File.ReadAllBytes("wwwroot/gallery/seed/bradley-dunn-nJvfIaChd_A-unsplash.jpg")
            };

            var car2 = new Car
            {
                Brand = "Audi",
                Model = "A4",
                Year = 2020,
                Horsepower = 220,
                Price = 35000,
                UserId = user2.Id,
                ImageData = File.ReadAllBytes("wwwroot/gallery/seed/tim-meyer-GIm7wxiAZys-unsplash.jpg")
            };

            db.Cars.AddRange(car1, car2);
            await db.SaveChangesAsync();

            db.ServiceRecords.AddRange(
                new ServiceRecord
                {
                    CarId = car1.Id,
                    ServiceDate = new DateTime(2023, 5, 1),
                    Description = "Oil change",
                    Mileage = 10000,
                    ServiceType = ServiceType.Maintenance
                },
                new ServiceRecord
                {
                    CarId = car2.Id,
                    ServiceDate = new DateTime(2023, 6, 1),
                    Description = "Tire replacement",
                    Mileage = 15000,
                    ServiceType = ServiceType.Repair
                }
            );
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        File.WriteAllText("seed-error.txt", $"ERROR: {ex.Message}\n\nStack: {ex.StackTrace}");
        throw;
    }
}
