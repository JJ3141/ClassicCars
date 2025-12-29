using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassicCars.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClassicCars.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollectionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            int carsPerPage = 24;

            var cars = _context.Cars
                .Skip((page - 1) * carsPerPage)
                .Take(carsPerPage)
                .ToList();

            int totalCars = _context.Cars.Count();
            int totalPages = (int)Math.Ceiling(totalCars / (double)carsPerPage);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(cars);
        }


    }
}

