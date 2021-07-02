using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Mappers.Order;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels.Cart;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Orders
{
    public class OrderApiClient : BaseSyncClient, IOrderService
    {
        public OrderApiClient(HttpClient client) : base(client, WebAPIInfo.Order) { }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{userName}").ConfigureAwait(false);
            return orders.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<Order> CreateOrder(string userName, CartWebModel cart, CreateOrderWebModel orderWebModel)
        {
            var model = new CreateOrderDTO
            {
                Order = orderWebModel,
                Items = cart.ToDTO(),
            };
            var response = await PostAsync($"{Address}/{userName}", model).ConfigureAwait(false);
            var order = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrderDTO>();
            return order.FromDTO();
        }
    }
}
