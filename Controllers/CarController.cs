using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace ClassicCars.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarsController(
            ICarService carService,
            UserManager<ApplicationUser> userManager)
        {
            _carService = carService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.GetAllAsync();
            return View(cars);
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
