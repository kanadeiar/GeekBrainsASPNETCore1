﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public BrandsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var brandsViews = _productData.GetBrandsWithProducts().OrderBy(b => b.Order).Select(b => new BrandWebModel
            {
                Id = b.Id,
                Name = b.Name,
                CountProduct = b.Products.Count,
            });
            return View(brandsViews);
        }
    }
}
