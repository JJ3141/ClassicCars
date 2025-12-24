using ClassicCars.Data;
using ClassicCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassicCars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index() {
            var cars = _context.Cars.ToList();
            return View(cars);
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null) return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
