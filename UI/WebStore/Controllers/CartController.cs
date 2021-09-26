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
            return View(new CartOrderWebModel{ Cart = _cartService.GetWebModel().Result });
        }
        public IActionResult Add(int id)
        {
            _cartService.Add(id);
            #region Лог
            _logger.LogInformation("Успешное добавление товара с идентификатором {0} 1 ед. в корзину покупателя", id);
            #endregion
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Subtract(int id)
        {
            _cartService.Subtract(id);
            #region Лог
            _logger.LogInformation("Успешное уменьшение товара с идентификатором {0} на 1 еденицу из корзины покупателя", id);
            #endregion
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            _cartService.Remove(id);
            #region Лог
            _logger.LogInformation("Успешное полное удаление товара с идентификатором {0} из корзины", id);
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
                    new CartOrderWebModel {Cart = _cartService.GetWebModel().Result, Order = model});

            var order = await _orderService.CreateOrder(User.Identity!.Name, _cartService.GetWebModel().Result, model);
            
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

        #region WebApi

        public IActionResult ApiGetCartView()
        {
            return ViewComponent("Cart");
        }

        public IActionResult ApiAdd(int id)
        {
            _cartService.Add(id);
            return Json(new {id, message = "Этот товар был успешно увеличен на еденицу либо добавлен"});
        }

        public IActionResult ApiSubtract(int id)
        {
            _cartService.Subtract(id);
            return Json(new {id, message = "Этот товар был успешно уменьшен на еденицу"});
        }

        public IActionResult ApiRemove(int id)
        {
            _cartService.Remove(id);
            return Json(new {id, message = "Этот товар был успешно удален из корзины"});
        }

        public IActionResult ApiClear()
        {
            _cartService.Clear();
            return Ok(new {message = "Корзина успешно очищена"});
        }

        #endregion
    }
}
