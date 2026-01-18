using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClassicCars.Controllers
{
    public class ServiceRecordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceRecord record)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var car = _context.Cars.FirstOrDefault(c => c.Id == record.CarId);

            if (car == null || car.UserId != userId)
                return Unauthorized();

            if (!ModelState.IsValid)
                return View(record);

            _context.ServiceRecords.Add(record);
            _context.SaveChanges();

            return RedirectToAction("Details", "Cars", new { id = record.CarId });
        }
        //public IActionResult Edit()
        //{
           
        //}

    }
}
