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
            var model = new User { Id = 0 };
            return View(model);
     
        }

        [HttpPost]
        public IActionResult Register(Register registerModel)
        {
            if (!ModelState.IsValid)
                return View(registerModel);


            bool isExist = _context.Users.Any(u => u.Email == registerModel.Email)||
                           _context.Users.Any(u => u.Username == registerModel.Username);
            if (isExist)
            {
                ModelState.AddModelError("", "Потребителят вече съществува.");
                return View(registerModel);
            }

            var user = new User
            {
                Username = registerModel.Username,
                Password = /*new PasswordHasher<User>().HashPassword(null,*/ registerModel.Password,
                Email=registerModel.Email,
                LastName=registerModel.LastName,
                FirstName=registerModel.FirstName
                
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login","Login");
        }
        public IActionResult Edit()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            return View("Register", user); 
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Save(User model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null) return NotFound();

            user.Username = model.Username;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Profile");
        }




    }
}

