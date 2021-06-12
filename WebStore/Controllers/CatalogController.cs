using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Infrastructure.Filters;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

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
            var filter = new ProductFilter()
            {
                SectionId = sectionId,
                BrandId = brandId,
            };

            var products = _productData.GetProducts(filter);

            var catalogView = new CatalogWebModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.Select(p => new ProductWebModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                }),
            };

            return View(catalogView);
        }
    }
}
