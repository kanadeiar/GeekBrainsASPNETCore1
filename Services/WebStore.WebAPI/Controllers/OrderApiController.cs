using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Mappers.Order;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    /// <summary> Управление заказами </summary>
    [Route(WebAPIInfo.ApiOrder), ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        /// <summary> Конструктор </summary>
        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary> Получение всех заказов указанного имени пользователя </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Список заказов</returns>
        [HttpGet("user/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDTO>))]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var orders = await _orderService.GetUserOrders(userName);
            return Ok(orders.ToDTO());
        }

        /// <summary> Все заказы всех пользователей </summary>
        /// <returns>Список заказов</returns>
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDTO>))]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders.ToDTO());
        }

        /// <summary> Получение заказа по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Один заказ</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            return Ok(order.ToDTO());
        }

        /// <summary> Создание нового заказа </summary>
        /// <param name="userName">Название заказа</param>
        /// <param name="orderDto">Детали заказа</param>
        /// <returns>Новый заказ</returns>
        [HttpPost("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody] CreateOrderDTO orderDto)
        {
            var order = await _orderService.CreateOrder(userName, orderDto.Items.FromDTO(), orderDto.Order);
            return Ok(order.ToDTO());
        }
    }
}
