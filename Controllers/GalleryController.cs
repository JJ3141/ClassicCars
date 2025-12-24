using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassicCars.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClassicCars.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Gallery()
        {
            return View();
        }
    }
}

