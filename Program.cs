using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services;
using ClassicCars.Services.Interfaces;
using ClassicCars.Repositories;
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
builder.Services.AddScoped<IWarrantyRepository, WarrantyRepository>();
builder.Services.AddScoped<WarrantyRepository>();
builder.Services.AddScoped<IWarrantyService, WarrantyService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
builder.Services.AddScoped<ICarReviewRepository, CarReviewRepository>();

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

app.UseExceptionHandler("/Error/500");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "warranty_create",
    pattern: "Cars/Warranty/{carId}",
    defaults: new { controller = "Warranty", action = "Create" }
);

app.MapControllerRoute(
    name: "warranty_delete",
    pattern: "Cars/Warranty/Delete/{carId}",
    defaults: new { controller = "Warranty", action = "Delete" }
);

app.MapControllerRoute(
    name: "create_warranty_legacy",
    pattern: "Cars/CreateWarranty",
    defaults: new { controller = "Warranty", action = "Create" }
);

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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

        var roles = new[] { "User", "Administrator" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!string.IsNullOrEmpty(user1?.Id) && !await userManager.IsInRoleAsync(user1, "User"))
        {
            await userManager.AddToRoleAsync(user1, "User");
        }

        if (!string.IsNullOrEmpty(user2?.Id) && !await userManager.IsInRoleAsync(user2, "Administrator"))
        {
            await userManager.AddToRoleAsync(user2, "Administrator");
        }

        if (!db.Cars.Any())
        {
            var car1 = new Car
            {
                Brand = "Chevrolet",
                Model = "Impala",
                Year = 1967,
                Horsepower = 300,
                Price = 50000,
                UserId = user1.Id,
                ImageData = File.ReadAllBytes("wwwroot/gallery/seed/bradley-dunn-nJvfIaChd_A-unsplash.jpg")
            };

            var car2 = new Car
            {
                Brand = "Chevrolet",
                Model = " Camaro ",
                Year = 2010,
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
