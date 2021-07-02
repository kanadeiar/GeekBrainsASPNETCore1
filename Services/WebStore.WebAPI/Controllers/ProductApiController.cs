using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Mappers;
using WebStore.Domain.Models;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIInfo.Product), ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductData _productData;
        public ProductApiController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet("section")]
        public IActionResult GetSections()
        {
            return Ok(_productData.GetSections().ToDTO());
        }

        [HttpGet("section/{id:int}")]
        public IActionResult GetSectionById(int id)
        {
            return Ok(_productData.GetSection(id).ToDTO());
        }

        [HttpGet("brand")]
        public IActionResult GetBrands()
        {
            return Ok(_productData.GetBrands().ToDTO());
        }

        [HttpGet("brand/{id:int}")]
        public IActionResult GetBrandById(int id)
        {
            return Ok(_productData.GetBrand(id).ToDTO());
        }

        [HttpPost]
        public IActionResult GetProducts(ProductFilter filter = null)
        {
            return Ok(_productData.GetProducts(filter, true).ToDTO());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            return Ok(_productData.GetProductById(id).ToDTO());
        }

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

        [HttpDelete("product/{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _productData.DeleteProduct(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
