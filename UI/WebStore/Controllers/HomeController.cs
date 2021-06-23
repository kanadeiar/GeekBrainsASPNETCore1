﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

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
        public IActionResult Index([FromServices] IProductData productData)
        {
            var products = _mapperProductToWeb
                .Map<IEnumerable<ProductWebModel>>(productData.GetProducts().Take(6));
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