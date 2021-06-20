using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.WebModels.Cart;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View(new CartOrderWebModel{Cart = _cartService.GetWebModel()});
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CreateOrderWebModel model, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), 
                    new CartOrderWebModel {Cart = _cartService.GetWebModel(), Order = model});

            var order = await _orderService.CreateOrder(User.Identity!.Name, _cartService.GetWebModel(), model);
            
            _cartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
        }

        public async Task<IActionResult> OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            ViewBag.Name = (await _orderService.GetOrderById(id)).Name;
            return View();
        }
    }
}
