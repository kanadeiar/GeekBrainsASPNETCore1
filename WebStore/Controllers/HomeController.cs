﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public IActionResult Index([FromServices] IProductData productData)
        {
            var products = productData
                .GetProducts()
                .Take(6)
                .Select(p => new ProductWebModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                });
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

        #endregion
    }
}
