using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        public CatalogController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index(int? brandId, int? sectionId)
        {
            return View();
        }
    }
}
