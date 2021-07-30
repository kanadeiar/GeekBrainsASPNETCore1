using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public BreadCrumbsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new BreadCrumbsWebModel();

            var action = ViewContext.RouteData.Values["action"] as string;
            if (ViewContext.RouteData.Values["controller"] is string controller && controller != "Home" )
            {
                model.Controller = controller;
                model.ControllerAction = controller switch
                {
                    "Account" => "Login",
                    "Cart" => "Index",
                    "Catalog" => "Index",
                    "Home" => "Index",
                    "UserProfile" => "Index",
                    "WebAPI" => "Index",
                    "Workers" => "Index",
                    _ => throw new ArgumentOutOfRangeException($"Недопустимый параметр название контроллера {nameof(controller)} значение = {controller}"),
                };
                model.ControllerText = controller switch
                {
                    "Account" => "Авторизация",
                    "Cart" => "Корзина",
                    "Catalog" => "Каталог",
                    "Home" => "Главная",
                    "UserProfile" => "Профиль",
                    "WebAPI" => "WebAPI",
                    "Workers" => "Работники",
                    _ => throw new ArgumentOutOfRangeException($"Недопустнымый параметр название контроллера {nameof(controller)} значение = {controller}"),
                };
            }
            if (int.TryParse(Request.Query["SectionId"], out var sectionId))
            {
                model.Section = await _productData.GetSection(sectionId);
                if (model.Section?.ParentId is { } parentSectionId)
                    model.Section.Parent = await _productData.GetSection(parentSectionId);
            }

            if (int.TryParse(Request.Query["BrandId"], out var brandId))
                model.Brand = await _productData.GetBrand(brandId);
            if (int.TryParse(ViewContext.RouteData.Values["Id"]?.ToString(), out var productId))
                model.Product = (await _productData.GetProductById(productId))?.Name;

            return View(model);
        }
    }
}
