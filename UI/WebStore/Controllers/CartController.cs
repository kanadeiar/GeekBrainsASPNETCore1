using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.WebModels.Cart;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, IOrderService orderService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(new CartOrderWebModel{Cart = _cartService.GetWebModel()});
        }
        public IActionResult Add(int id)
        {
            _cartService.Add(id);
            #region Лог
            _logger.LogInformation($"Успешное добавление товара с идентификатором {id} 1 ед. в корзину покупателя");
            #endregion
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Subtract(int id)
        {
            _cartService.Subtract(id);
            #region Лог
            _logger.LogInformation($"Успешное уменьшение товара с идентификатором {id} на 1 еденицу из корзины покупателя");
            #endregion
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            _cartService.Remove(id);
            #region Лог
            _logger.LogInformation($"Успешное полное удаление товара с идентификатором {id} из корзины");
            #endregion
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Clear()
        {
            _cartService.Clear();
            #region Лог
            _logger.LogInformation("Успешная очистка корзины покупателя от товаров");
            #endregion
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CreateOrderWebModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), 
                    new CartOrderWebModel {Cart = _cartService.GetWebModel(), Order = model});

            var order = await _orderService.CreateOrder(User.Identity!.Name, _cartService.GetWebModel(), model);
            
            _cartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
        }

        [Authorize]
        public async Task<IActionResult> OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            ViewBag.Name = (await _orderService.GetOrderById(id)).Name;
            #region Лог
            _logger.LogInformation("Успешное оформление заказа на основе данных корзины");
            #endregion
            return View();
        }
    }
}
