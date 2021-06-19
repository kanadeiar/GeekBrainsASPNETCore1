using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Services
{
    public class DatabaseOrderService : IOrderService
    {
        public Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            
        }

        public Task<Order> GetOrderById(int id)
        {
            
        }

        public Task<Order> CreateOrder(string userName, CartWebModel Cart, CreateOrderViewModel model)
        {
            
        }
    }
}
