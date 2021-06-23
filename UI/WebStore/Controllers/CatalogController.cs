using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Infrastructure.Filters;
using WebStore.Domain.WebModels;
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
                Products = _mapperProductToWeb
                    .Map<IEnumerable<ProductWebModel>>(products),
            };

            return View(catalogView);
        }

        public IActionResult Details(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null)
                return NotFound();

            return View(_mapperProductToWeb
                .Map<ProductWebModel>(product));
        }
    }
}
