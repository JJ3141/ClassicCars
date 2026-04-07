using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
namespace ClassicCars.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
    private readonly IWarrantyService _warrantyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CarsController> _logger;

        public CarsController(
            ICarService carService,
            UserManager<ApplicationUser> userManager,
            IWarrantyService warrantyService,
            ILogger<CarsController> logger)
        {
            _carService = carService;
            _userManager = userManager;
            _warrantyService = warrantyService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.GetAllAsync();
            return View(cars);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Warranty(int id)
        {
            var carDetails = await _carService.GetDetailsAsync(id);
            if (carDetails == null)
                return NotFound();

            var dto = await _warrantyService.GetByCarIdAsync(id);

            var vm = new ClassicCars.WarrantyViewModel
            {
                CarId = id,
                Car = new ClassicCars.Models.Car
                {
                    Id = carDetails.Id,
                    Brand = carDetails.Brand,
                    Model = carDetails.Model,
                    Year = carDetails.Year
                },
                HasWarranty = dto != null
            };

            if (dto != null)
            {
                vm.Provider = dto.Provider;
                vm.CoverageAmount = dto.CoverageAmount;
                vm.StartDate = dto.StartDate;
                vm.EndDate = dto.EndDate;
                vm.Notes = dto.Notes;
            }

            return View("Warranty", vm);
        }

        public async Task<IActionResult> CreateWarranty(int carId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var carDetails = await _carService.GetDetailsAsync(carId);
            if (carDetails == null || carDetails.UserId != currentUser?.Id)
                return Forbid();

            var vm = new ClassicCars.WarrantyViewModel { CarId = carId, Car = new ClassicCars.Models.Car { Id = carId, Brand = carDetails.Brand, Model = carDetails.Model, Year = carDetails.Year } };
            return View("~/Views/Warranty/Add.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWarranty(ClassicCars.WarrantyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                _logger.LogWarning("CreateWarranty ModelState invalid for CarId {CarId}: {Errors}", vm.CarId, string.Join("; ", errors));
                return View("~/Views/Warranty/Add.cshtml", vm);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var carDetails = await _carService.GetDetailsAsync(vm.CarId);
            if (carDetails == null || carDetails.UserId != currentUser?.Id)
            {
                _logger.LogWarning("User {UserId} attempted to add warranty for CarId {CarId} but is not the owner or car not found.", currentUser?.Id, vm.CarId);
                return Forbid();
            }

            var dto = new ClassicCars.Dtos.WarrantyDto
            {
                CarId = vm.CarId,
                Provider = vm.Provider,
                CoverageAmount = vm.CoverageAmount,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                Notes = vm.Notes
            };

            try
            {
                await _warrantyService.AddWarrantyAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add warranty for CarId {CarId}", vm.CarId);
                ModelState.AddModelError(string.Empty, "An error occurred while saving the warranty.");
                return View("~/Views/Warranty/Add.cshtml", vm);
            }

            return RedirectToAction("Details", new { id = vm.CarId });
        }


        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetDetailsAsync(id);

            if (car == null)
                return NotFound();

            return View(car);
        }
        public async Task<IActionResult> ReviewDetails(int id)
        {
            var car = await _carService.GetDetailsAsync(id);

            if (car == null)
                return NotFound();

            return View(car);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carService.GetDetailsAsync(id);

            if (car == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (user == null || car.UserId != user.Id)
                return Forbid();

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var success = await _carService.DeleteAsync(id, user.Id);

            if (!success)
                return Forbid();

            return RedirectToAction(nameof(Index));
        }

      
    }
}
