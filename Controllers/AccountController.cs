using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Controllers
{
	public class AccountController:Controller
	{
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

         
             bool IsExist = _context.Users.Any(u => u.Username == registerUser.Username);
            if (IsExist)
                return Conflict(new { message = "Потребителят вече съществува." });

            _context.Users.Add(registerUser);
            _context.SaveChanges();

            return Ok(new { message = "Регистрация успешна." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user == null)
                return Unauthorized(new { message = "Грешно потребителско име или парола." });
            return Ok(new { message = "Успешен вход." });
        }


    }
}

