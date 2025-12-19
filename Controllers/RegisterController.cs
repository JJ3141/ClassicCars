using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClassicCars.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register registerModel)
        {
            if (!ModelState.IsValid)
                return View(registerModel);


            bool isExist = _context.Users.Any(u => u.Username == registerModel.Username);
            if (isExist)
            {
                ModelState.AddModelError("", "Потребителят вече съществува.");
                return View(registerModel);
            }

            var user = new User
            {
                Username = registerModel.Username,
                Password = /*new PasswordHasher<User>().HashPassword(null,*/ registerModel.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}

