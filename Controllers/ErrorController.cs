using Microsoft.AspNetCore.Mvc;

namespace ClassicCars.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            return View("ServerError");
        }

        [Route("Error/{code}")]
        public IActionResult Handle(int code)
        {
            if (code == 404) return View("NotFound");
            return View("ServerError");
        }
    }
}