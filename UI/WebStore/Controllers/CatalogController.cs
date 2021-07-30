using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        private readonly Mapper _mapperProductToWeb = new(new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
                .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
                .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));
        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(int? brandId, int? sectionId, int page = 1, int? PageSize = null)
        {
            var pageSize = PageSize ?? (int.TryParse(_configuration["CatalogPageSize"], out var value) ? value : null);

            var filter = new ProductFilter()
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = pageSize,
            };

            var productsPage = await _productData.GetProducts(filter);
            var products = productsPage.Products;

            var catalogView = new CatalogWebModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                PageWebModel = new PageWebModel(productsPage.TotalCount, page, pageSize ?? 0),
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
