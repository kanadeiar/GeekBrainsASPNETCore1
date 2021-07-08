using Microsoft.AspNetCore.Mvc;
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
        public ProductApiController(IProductData productData)
        {
            _productData = productData;
        }

        /// <summary> Получить все категории товаров </summary>
        /// <returns>Категории товаров</returns>
        [HttpGet("section")]
        public IActionResult GetSections()
        {
            return Ok(_productData.GetSections().ToDTO());
        }

        /// <summary> Получить категорию по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Категория товара</returns>
        [HttpGet("section/{id:int}")]
        public IActionResult GetSectionById(int id)
        {
            return Ok(_productData.GetSection(id).ToDTO());
        }

        /// <summary> Получить все бренды товаров </summary>
        /// <returns>Бренды товаров</returns>
        [HttpGet("brand")]
        public IActionResult GetBrands()
        {
            return Ok(_productData.GetBrands().ToDTO());
        }

        /// <summary> Получить бренд по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Бренд</returns>
        [HttpGet("brand/{id:int}")]
        public IActionResult GetBrandById(int id)
        {
            return Ok(_productData.GetBrand(id).ToDTO());
        }

        /// <summary> Получить товары используя фильтр </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns>Товары</returns>
        [HttpPost]
        public IActionResult GetProducts(ProductFilter filter = null)
        {
            return Ok(_productData.GetProducts(filter, true).ToDTO());
        }

        /// <summary> Получить товар по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Товар</returns>
        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            return Ok(_productData.GetProductById(id).ToDTO());
        }

        /// <summary> Добавить один товар </summary>
        /// <param name="product">Этод добавляемый товар</param>
        /// <returns>Идентификатор добавленного товара</returns>
        [HttpPost("product")]
        public IActionResult Add(ProductDTO product)
        {
            //var id = _productData.AddProduct(product.FromDTO());
            var prod = product.FromDTO();
            prod.SectionId = prod.Section.Id;
            prod.Section = null;
            prod.BrandId = prod.Brand.Id;
            prod.Brand = null;
            var id = _productData.AddProduct(prod);
            return Ok(id);
        }

        /// <summary> Обновить один товар </summary>
        /// <param name="product">Обновленный товар</param>
        /// <returns>Результат работы</returns>
        [HttpPut("product")]
        public IActionResult Update(ProductDTO product)
        {
            //_productData.UpdateProduct(product.FromDTO());
            var prod = product.FromDTO();
            prod.SectionId = prod.Section.Id;
            prod.Section = null;
            prod.BrandId = prod.Brand.Id;
            prod.Brand = null;
            _productData.UpdateProduct(prod);
            return Ok();
        }

        /// <summary> Удалить один товар </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Получилось ли удалить товар</returns>
        [HttpDelete("product/{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _productData.DeleteProduct(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
