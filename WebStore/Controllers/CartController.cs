using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            return View(_cartService.GetWebModel());
        }
        public IActionResult Add(int id)
        {
            _cartService.Add(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Subtract(int id)
        {
            _cartService.Subtract(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            _cartService.Remove(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction("Index", "Cart");
        }
    }
}
