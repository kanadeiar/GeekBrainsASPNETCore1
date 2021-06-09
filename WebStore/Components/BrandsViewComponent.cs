using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

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
            var brandsViews = _productData.GetBrands().OrderBy(b => b.Order).Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
            });
            return View(brandsViews);
        }
    }
}
