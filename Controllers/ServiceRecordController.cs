using System.Security.Claims;
using ClassicCars.Models;
using ClassicCars.Services.Interfaces;
using ClassicCars.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ClassicCars.Controllers {
    [Authorize]
    public class ServiceRecordController : Controller
    {
        private readonly IServiceRecordService _serviceRecordService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceRecordController(
            IServiceRecordService serviceRecordService,
            UserManager<ApplicationUser> userManager)
        {
            _serviceRecordService = serviceRecordService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRecordViewModel record)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var success = await _serviceRecordService
                .AddServiceRecordAsync(record, user.Id);

            if (!success)
                return Unauthorized();

            return RedirectToAction("Details", "Cars",
                new { id = record.CarId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceRecordViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var success = await _serviceRecordService.EditAsync(model, userId);

            if (!success)
                return NotFound();

            return RedirectToAction("Details", "Cars", new { id = model.CarId });
        }
    }


}
