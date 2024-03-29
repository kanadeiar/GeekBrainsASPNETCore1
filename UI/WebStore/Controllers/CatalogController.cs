﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Mappers;
using WebStore.Domain.WebModels.Product;
using WebStore.Domain.WebModels.Shared;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }

        /// <summary> Главная страница каталога товаров, пагинованная </summary>
        public async Task<IActionResult> Index(int? brandId, int? sectionId, int page = 1)
        {
            var model = await GetCatalogWebModel(brandId, sectionId, page);
            return View(model);
        }

        /// <summary> Детальные данные по каждому товару </summary>
        public async Task<IActionResult> Details(int id)
        {
            var productPage = await _productData.GetProducts();
            var product = await _productData.GetProductById(id);
            if (product is null)
                return NotFound();
            var products = productPage.Products.ToWeb();
            ViewBag.CatagoryProducts = new[]
            {
                products.Take(4),
                products.Skip(4).Take(4),
                products.Skip(2).Take(4),
            };
            ViewBag.RecommendedProducts = new[]
            {
                products.Take(3),
                products.Skip(3).Take(3),
            };

            return View(product.ToWeb());
        }

        #region WebApi

        /// <summary> Частичное представление с отфильтрованными и пагинованными товарами </summary>
        public async Task<IActionResult> ApiGetProductPartialView(int? BrandId, int? SectionId, int Page = 1)
        {
            var products = await GetProducts(BrandId, SectionId, Page);
            return PartialView("Partial/_ProductsPartial", products);
        }

        /// <summary> Частичное представление с пагинацией по товарам </summary>
        public async Task<IActionResult> ApiGetCatalogPaginationPartialView(int? BrandId, int? SectionId, int Page = 1)
        {
            var model = await GetCatalogWebModel(BrandId, SectionId, Page);
            return PartialView("Partial/_CatalogPaginationPartial", model);
        }
        
        #endregion

        #region Вспомогательные

        private async Task<CatalogWebModel> GetCatalogWebModel(int? BrandId, int? SectionId, int Page)
        {
            var filter = GetProductFilter(BrandId, SectionId, Page);
            var (products, productCount) = await _productData.GetProducts(filter);

            var model = new CatalogWebModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                PageWebModel = new PageWebModel(productCount, Page, filter.PageSize ?? 6),
                Products = products.OrderBy(p => p.Order).ToWeb(),
            };
            return model;
        }

        private async Task<IEnumerable<ProductWebModel>> GetProducts(int? BrandId, int? SectionId, int Page)
        {
            var filter = GetProductFilter(BrandId, SectionId, Page);
            var result = (await _productData.GetProducts(filter)).Products.OrderBy(p => p.Order);

            return result.ToWeb();
        }

        private ProductFilter GetProductFilter(int? BrandId, int? SectionId, int Page)
        {
            var pageSize = (int.TryParse(_configuration["CatalogPageSize"], out var value) ? value : 6);
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = pageSize,
            };
            return filter;
        }

        #endregion
    }
}
