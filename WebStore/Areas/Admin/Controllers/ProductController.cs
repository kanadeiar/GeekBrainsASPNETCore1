using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrators)]
    public class ProductController : Controller
    {
        private readonly IProductData _ProductData;

        private readonly Mapper _mapperProductToWeb =
            new(new MapperConfiguration(c => c.CreateMap<Product, ProductEditWebModel>()            
                .ForMember("SectionName", o => o.MapFrom(p => p.Section.Name))
                .ForMember("BrandName", o => o.MapFrom(p => p.Brand.Name))));
        private readonly Mapper _mapperProductFromWeb =
            new(new MapperConfiguration(c => c.CreateMap<ProductEditWebModel, Product>()));

        public ProductController(IProductData productData)
        {
            _ProductData = productData;
        }
        public IActionResult Index()
        {
            var model = _mapperProductToWeb.Map<IEnumerable<ProductEditWebModel>>(_ProductData.GetProducts(includes:true)
                .OrderBy(p => p.Order));
            return View(model);
        }

        public IActionResult Create() => View("Edit", new ProductEditWebModel());

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (_ProductData.GetProductById(id) is { } product)
            {
                return View(_mapperProductToWeb.Map<ProductEditWebModel>(product));
            }
            return NotFound();
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (_ProductData.GetProductById(id) is { } product)
            {
                return View(_mapperProductToWeb.Map<ProductEditWebModel>(product));
            }
            return NotFound();
        }

    }
}
