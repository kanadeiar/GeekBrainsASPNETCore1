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
using Microsoft.Extensions.Logging;
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
        private readonly IProductData _productData;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<ProductController> _logger;

        private readonly Mapper _mapperProductToWeb =
            new(new MapperConfiguration(c => c.CreateMap<Product, EditProductWebModel>()            
                .ForMember("SectionName", o => o.MapFrom(p => p.Section.Name))
                .ForMember("BrandName", o => o.MapFrom(p => p.Brand.Name))));
        private readonly Mapper _mapperProductFromWeb;

        public ProductController(IProductData productData, IWebHostEnvironment appEnvironment, ILogger<ProductController> logger)
        {
            _productData = productData;
            _appEnvironment = appEnvironment;
            _logger = logger;

            _mapperProductFromWeb = new(new MapperConfiguration(c => c.CreateMap<EditProductWebModel, Product>()
                .ForMember("Section", o => o.MapFrom(p => _productData.GetSection((int) p.SectionId).Result))
                .ForMember("Brand", o => o.MapFrom(p => _productData.GetBrand((int) p.BrandId).Result))));
        }

        /// <summary> Обзор всех товаров в админке </summary>
        /// <param name="name">Фильтр по названию</param> <param name="page">Страница в пагинаторе</param> <param name="sortOrder">Вид сортировки</param>
        public async Task<IActionResult> Index(string name, int page = 1, ProductSortState sortOrder = ProductSortState.NameAsc)
        {
            int pageSize = 9;
            var filter = new ProductFilter
            {
                Page = page,
                PageSize = pageSize,
            };
            var productsPage = await _productData.GetProducts(filter);
            var products = productsPage.Products;

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
            var webModel = new ProductPageWebModel
            {
                Filter = new ProductFilterWebModel(name),
                Sort = new ProductSortWebModel(sortOrder),
                Page = new PageWebModel(productsPage.TotalCount, page, pageSize),
                Products = _mapperProductToWeb.Map<IEnumerable<EditProductWebModel>>(products.ToList()),
            };
            return View(webModel);
        }

        /// <summary> Создание нового товара </summary>
        public async Task<IActionResult> Create()
        {
            ViewBag.Sections = new SelectList(await _productData.GetSections(), "Id", "Name");
            ViewBag.Brands = new SelectList(await _productData.GetBrands(), "Id", "Name");
            return View("Edit", new EditProductWebModel());
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (await _productData.GetProductById(id) is { } product)
            {
                ViewBag.Sections = new SelectList(await _productData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(await _productData.GetBrands(), "Id", "Name");
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
                ViewBag.Sections = new SelectList(await _productData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(await _productData.GetBrands(), "Id", "Name");
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
                await _productData.AddProduct(product);
            else
                await _productData.UpdateProduct(product);

            return RedirectToAction("Index", "Product");
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (await _productData.GetProductById(id) is { } product)
            {
                ViewBag.Sections = new SelectList(await _productData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(await _productData.GetBrands(), "Id", "Name");
                return View(_mapperProductToWeb.Map<EditProductWebModel>(product));
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();
            var result = await _productData.DeleteProduct(id);
            if (!result)
                _logger.LogError($"Не удалось удалить товар с id={id}");

            return RedirectToAction("Index", "Product");
        }
    }
}
