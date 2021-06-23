using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        /// <summary> Получить все заказы пользователя </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Заказы пользователя</returns>
        Task<IEnumerable<Order>> GetUserOrders(string userName);

        /// <summary> Получить заказ по идентификатору </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>Заказ</returns>
        Task<Order> GetOrderById(int id);

        /// <summary> Создание казаза нового </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="Cart">Корзина пользователя</param>
        /// <param name="model">Данные заказа с веба</param>
        /// <returns>Новый заказ</returns>
        Task<Order> CreateOrder(string userName, CartWebModel Cart, CreateOrderWebModel model);
    }
}
