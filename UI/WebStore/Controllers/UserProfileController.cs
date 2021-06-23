using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Identity;
using WebStore.Domain.WebModels.UserProfile;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Mapper _mapperOrderToView = 
            new (new MapperConfiguration(c => c.CreateMap<Order, UserOrderWebModel>()
                .ForMember("PriceSum", o => o
                    .MapFrom(u => u.Items.Sum(i => i.Price)))
                .ForMember("Count", o => o.MapFrom(u => u.Items.Count))));
        public UserProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrders(User.Identity!.Name);
            return View(_mapperOrderToView.Map<IEnumerable<UserOrderWebModel>>(orders));
        }
    }
}
