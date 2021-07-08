using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels.Product;
using WebStore.Domain.WebModels.Shared;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrators)]
    public class ProductController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IWebHostEnvironment _appEnvironment;

        private readonly Mapper _mapperProductToWeb =
            new(new MapperConfiguration(c => c.CreateMap<Product, EditProductWebModel>()            
                .ForMember("SectionName", o => o.MapFrom(p => p.Section.Name))
                .ForMember("BrandName", o => o.MapFrom(p => p.Brand.Name))));
        private readonly Mapper _mapperProductFromWeb;

        public ProductController(IProductData productData, IWebHostEnvironment appEnvironment)
        {
            _ProductData = productData;
            _appEnvironment = appEnvironment;
            _mapperProductFromWeb = new(new MapperConfiguration(c => c.CreateMap<EditProductWebModel, Product>()
                .ForMember("Section", o => o.MapFrom(p => _ProductData.GetSection((int) p.SectionId)))
                .ForMember("Brand", o => o.MapFrom(p => _ProductData.GetBrand((int) p.BrandId)))));
        }
        public IActionResult Index(string name, int page = 1, ProductSortState sortOrder = ProductSortState.NameAsc)
        {
            var products = _ProductData.GetProducts(new ProductFilter { Name = name }, true);

            var pageSize = 10;
            var count = products!.Count();
            products = products!.Skip((page - 1) * pageSize).Take(pageSize);

            products = sortOrder switch
            {
                ProductSortState.NameAsc => products.OrderBy(p => p.Name),
                ProductSortState.NameDesc => products.OrderByDescending(p => p.Name),
                ProductSortState.OrderAsc => products.OrderBy(p => p.Order),
                ProductSortState.OrderDesc => products.OrderByDescending(p => p.Order),
                ProductSortState.PriceAsc => products.OrderBy(p => p.Price),
                ProductSortState.PriceDesc => products.OrderByDescending(p => p.Price),
                ProductSortState.SectionAsc => products.OrderBy(p => p.Section.Name),
                ProductSortState.SectionDesc => products.OrderByDescending(p => p.Section.Name),
                ProductSortState.BrandAsc => products.OrderBy(p => p.Brand.Name),
                ProductSortState.BrandDesc => products.OrderByDescending(p => p.Brand.Name),
                _ => products.OrderBy(p => p.Name),
            };
            var webModel = new ProductIndexWebModel
            {
                Filter = new ProductFilterWebModel(name),
                Sort = new ProductSortWebModel(sortOrder),
                Page = new PageWebModel(count, page, pageSize),
                Products = _mapperProductToWeb.Map<IEnumerable<EditProductWebModel>>(products.ToList()),
            };
            return View(webModel);
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
        public async Task<IActionResult> Edit(EditProductWebModel model, IFormFile uploadedFile)
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

            if (uploadedFile is not null)
            {
                string path = "/images/shop/" + uploadedFile.FileName;
                await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await uploadedFile.CopyToAsync(fileStream);
                product.ImageUrl = uploadedFile.FileName;
            }

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
