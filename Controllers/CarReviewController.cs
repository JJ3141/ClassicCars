using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassicCars.Controllers
{
    [Authorize]
    public class CarReviewController : Controller
    {
        private readonly ICarReviewService _reviewService;

        public CarReviewController(ICarReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int carId, CarReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Cars", new { id = carId });
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
            await _reviewService.AddReviewAsync(carId, userId, model);

            return RedirectToAction("Details", "Cars", new { id = carId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int reviewId, int carId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
            await _reviewService.DeleteReviewAsync(reviewId, userId);

            return RedirectToAction("Details", "Cars", new { id = carId });
        }
    }
}