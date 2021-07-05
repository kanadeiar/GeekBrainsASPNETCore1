using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Mappers.Order;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIInfo.ApiOrder), ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var orders = await _orderService.GetUserOrders(userName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            return Ok(order.ToDTO());
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody] CreateOrderDTO orderDto)
        {
            var order = await _orderService.CreateOrder(userName, orderDto.Items.FromDTO(), orderDto.Order);
            return Ok(order.ToDTO());
        }
    }
}
