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
        public async Task<IActionResult> Edit([Bind(Prefix = "EditCar")] EditCarViewModel car)
        {
            if (!ModelState.IsValid)
            {
                var profile = await _profileService.GetProfileAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (profile == null)
                    return RedirectToAction("Login", "Account");

                // preserve posted edit model so values and validation messages are shown
                profile.EditCar = car;
                return View("Index", profile);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var owned = await _profileService.GetCarByIdAsync(car.Id, userId);
            if (owned == null)
            {
                // not found or not owned by current user
                return NotFound();
            }

            await _carService.EditCarAsync(car);

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "NewCar")] CarCreateViewModel car)
        {
       
            if (!ModelState.IsValid)
            {
                var profile = await _profileService.GetProfileAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (profile == null)
                    return RedirectToAction("Login", "Account");

                profile.NewCar = car;
                return View("Index", profile);
            }

            var result = await _carService.AddCarAsync(car, User, true);

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
