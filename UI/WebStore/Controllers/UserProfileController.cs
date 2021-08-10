using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels.UserProfile;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    /// <summary> Контроллер отображения данных о профиле пользователя </summary>
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly Mapper _mapperOrderToView = 
            new (new MapperConfiguration(c => c.CreateMap<Order, UserOrderWebModel>()
                .ForMember("PriceSum", o => o.MapFrom(u => u.Items.Sum(i => i.Price * i.Quantity)))
                .ForMember("Count", o => o.MapFrom(u => u.Items.Sum(i => i.Quantity)))));
        /// <summary> Конструктор </summary>
        public UserProfileController()
        {
        }
        /// <summary> Главная страница пользователя </summary>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary> Заказы пользователя </summary>
        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrders(User.Identity!.Name);
            return View(_mapperOrderToView.Map<IEnumerable<UserOrderWebModel>>(orders));
        }

        [AllowAnonymous]
        /// <summary> Желаемые товары пользователя </summary>
        public async Task<IActionResult> Wanteds([FromServices] IWantedService wantedService)
        {
            var model = await wantedService.GetWebModel();
            return View(model);
        }

        [AllowAnonymous]
        /// <summary> Добавить в список желаемых товаров </summary>
        public IActionResult WantedAdd(int id, [FromServices] IWantedService wantedService)
        {
            wantedService.Add(id);
            return RedirectToAction("Wanteds");
        }
        /// <summary> Удаление из списка желаемого товара </summary>
        [AllowAnonymous]
        public IActionResult WantedRemove(int id, [FromServices] IWantedService wantedService)
        {
            wantedService.Remove(id);
            return RedirectToAction("Wanteds");
        }
        /// <summary> Очистка списка желаемых товаров </summary>
        [AllowAnonymous]
        public IActionResult WantedClear([FromServices] IWantedService wantedService)
        {
            wantedService.Clear();
            return RedirectToAction("Wanteds");
        }
        /// <summary> Добавление товара к сравнению </summary>
        [AllowAnonymous]
        public IActionResult CompareAdd(int id, string returnUrl, [FromServices] ICompareService compareService)
        {
            var (result, model) = compareService.AddAndGetWebModel(id);
            if (result)
            {
                compareService.Clear();
                return View(model);
            }
            else
            {
                return LocalRedirect(returnUrl ?? "/");
            }
        }
    }
}
