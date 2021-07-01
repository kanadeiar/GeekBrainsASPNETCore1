using Microsoft.AspNetCore.Mvc;
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





    }
}
