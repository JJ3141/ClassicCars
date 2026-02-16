using ClassicCars.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ClassicCars.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _collectionService
                .GetPagedCarsAsync(page);

            return View(model);
        }
    }
}
