using System.Security.Claims;
using ClassicCars.Data;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassicCars.Controllers
{
    public class ProfileController : Controller
    {

        private readonly IProfileService _carService;
        private readonly ApplicationDbContext _context;

        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(
            IProfileService profileService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IProfileService carService)
        {
            _profileService = profileService;
            _userManager = userManager;
            _context = context;
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var profile = await _profileService.GetProfileAsync(user.Id);
            return View(profile);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCarViewModel car)
        {
            if (!ModelState.IsValid) {
                var profile = await _profileService.GetProfileAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("Index", profile);

            }
           

            await _carService.EditCarAsync(car);

            return RedirectToAction("Index");


        }


        public async Task<IActionResult> Create(CarCreateViewModel car)
        {
            var result = await _carService.AddCarAsync(car, User, ModelState.IsValid);

            if (result == "Login")
                return RedirectToAction("Login", "Account");

            if (result == "Index")
                return RedirectToAction(nameof(Index));

            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var result = await _carService.DeleteCarAsync(id, userId);

                if (!result)
                {
                    return NotFound();
                }

                return RedirectToAction("Index", "Profile");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

    }
}
