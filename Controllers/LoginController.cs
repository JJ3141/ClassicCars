using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClassicCars.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users
                .FirstOrDefault(u => u.Username == model.Username
                                  && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Грешно потребителско име или парола.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}

