using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            new(new MapperConfiguration(c => c.CreateMap<Product, EditProductWebModel>()            
                .ForMember("SectionName", o => o.MapFrom(p => p.Section.Name))
                .ForMember("BrandName", o => o.MapFrom(p => p.Brand.Name))));
        private readonly Mapper _mapperProductFromWeb =
            new(new MapperConfiguration(c => c.CreateMap<EditProductWebModel, Product>()));

        public ProductController(IProductData productData)
        {
            _ProductData = productData;
        }
        public IActionResult Index()
        {
            var model = _mapperProductToWeb.Map<IEnumerable<EditProductWebModel>>(_ProductData.GetProducts(includes:true)
                .OrderBy(p => p.Order));
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
            ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
            return View("Edit", new EditProductWebModel());
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (_ProductData.GetProductById(id) is { } product)
            {
                ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
                return View(_mapperProductToWeb.Map<EditProductWebModel>(product));
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditProductWebModel model)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
                return View(model);
            }

            var product = _mapperProductFromWeb.Map<Product>(model);

            if (product.Id == 0)
                _ProductData.AddProduct(product);
            else
                _ProductData.UpdateProduct(product);

            return RedirectToAction("Index", "Product");
        }
        
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (_ProductData.GetProductById(id) is { } product)
            {
                ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
                return View(_mapperProductToWeb.Map<EditProductWebModel>(product));
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();
            _ProductData.DeleteProduct(id);

            return RedirectToAction("Index", "Product");
        }
    }
}
