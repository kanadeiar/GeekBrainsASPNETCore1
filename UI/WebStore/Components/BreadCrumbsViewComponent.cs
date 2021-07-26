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
