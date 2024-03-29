﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Mappers;
using WebStore.Domain.Models;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    /// <summary> Товары </summary>
    [Route(WebAPIInfo.ApiProduct), ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductData _productData;
        /// <summary> Конструктор </summary>
        public ProductApiController(IProductData productData)
        {
            _productData = productData;
        }

        /// <summary> Получить все категории товаров </summary>
        /// <returns>Категории товаров</returns>
        [HttpGet("Section")]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _productData.GetSections();
            return Ok(sections.ToDTO());
        }

        /// <summary> Получить категорию по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Категория товара</returns>
        [HttpGet("Section/{id:int}")]
        public async Task<IActionResult> GetSectionById(int id)
        {
            var section = await _productData.GetSection(id);
            return Ok(section.ToDTO());
        }

        /// <summary> Получить все бренды товаров </summary>
        /// <returns>Бренды товаров</returns>
        [HttpGet("Brand")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _productData.GetBrands();
            return Ok(brands.ToDTO());
        }

        /// <summary> Получить бренд по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Бренд</returns>
        [HttpGet("Brand/{id:int}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _productData.GetBrand(id);
            return Ok(brand.ToDTO());
        }

        /// <summary> Получить ключевые слова </summary>
        /// <returns>Ключевые слова</returns>
        [HttpGet("Tag")]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _productData.GetTags();
            return Ok(tags.ToDto());
        }

        /// <summary> Получить ключевое слово по идентификатору </summary>
        /// <param name="id">Ключевое слово</param>
        [HttpGet("Tag/{id:int}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _productData.GetTag(id);
            return Ok(tag.ToDto());
        }

        /// <summary> Получить товары используя фильтр </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Товары</returns>
        [HttpPost]
        public async Task<IActionResult> GetProducts(ProductFilter filter = null)
        {
            var products = await _productData.GetProducts(filter, true);
            return Ok(products.ToDTO());
        }

        /// <summary> Получить товар по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Товар</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productData.GetProductById(id);
            return Ok(product.ToDTO());
        }

        /// <summary> Добавить один товар </summary>
        /// <param name="product">Этод добавляемый товар</param>
        /// <returns>Идентификатор добавленного товара</returns>
        [HttpPost("Product")]
        public async Task<IActionResult> Add(ProductDTO product)
        {
            var prod = product.FromDTO();
            prod.SectionId = prod.Section.Id;
            prod.Section = null;
            prod.BrandId = prod.Brand.Id;
            prod.Brand = null;
            var id = await _productData.AddProduct(prod);
            return Ok(id);
        }

        /// <summary> Обновить один товар </summary>
        /// <param name="product">Обновленный товар</param>
        /// <returns>Результат работы</returns>
        [HttpPut("Product")]
        public async Task<IActionResult> Update(ProductDTO product)
        {
            var prod = product.FromDTO();
            prod.SectionId = prod.Section.Id;
            prod.Section = null;
            prod.BrandId = prod.Brand.Id;
            prod.Brand = null;
            await _productData.UpdateProduct(prod);
            return Ok();
        }

        /// <summary> Удалить один товар </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Получилось ли удалить товар</returns>
        [HttpDelete("Product/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productData.DeleteProduct(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
