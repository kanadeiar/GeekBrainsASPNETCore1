using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.ProductsInCartCount = (await _cartService.GetWebModel()).ItemsSum;
            return View();
        }
    }
}
