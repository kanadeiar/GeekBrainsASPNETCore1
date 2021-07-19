using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Shared;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly Mapper _mapperProductToWeb = new(new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
                .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
                .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));
        public CatalogController(IProductData productData)
        {
            _productData = productData;
        }
        public async Task<IActionResult> Index(int? brandId, int? sectionId, int page = 1)
        {
            var filter = new ProductFilter()
            {
                SectionId = sectionId,
                BrandId = brandId,
            };

            var products = await _productData.GetProducts(filter);

            int pageSize = 9;
            var count = products.Count();
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            var catalogView = new CatalogWebModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                PageWebModel = new PageWebModel(count, page, pageSize),
                Products = _mapperProductToWeb
                    .Map<IEnumerable<ProductWebModel>>(products),
            };

            return View(catalogView);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productData.GetProductById(id);
            if (product is null)
                return NotFound();

            return View(_mapperProductToWeb
                .Map<ProductWebModel>(product));
        }
    }
}
