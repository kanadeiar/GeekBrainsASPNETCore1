using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.Entities;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly Mapper _mapperProductToWeb = new(new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));
        public HomeController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public async Task<IActionResult> Index([FromServices] IProductData productData)
        {
            var gettedProducts = await productData.GetProducts();
            var products = _mapperProductToWeb
                .Map<IEnumerable<ProductWebModel>>(gettedProducts.Products.Take(6));
            ViewBag.Products = products;
            return View();
        }
        public IActionResult ProductDetails()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogSingle()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        
        #region Вспомогательные

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Second(string message)
        {
            return Content(message);
        }

        public IActionResult Throw(string id) => 
            throw new ApplicationException(id);

        #endregion
    }
}
