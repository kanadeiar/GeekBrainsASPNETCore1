using System.Collections.Generic;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Domain.DTO.Order
{
    /// <summary> Информация о создаваемом заказе </summary>
    public class CreateOrderDTO
    {
        /// <summary> Данные заказа </summary>
        public CreateOrderWebModel Order { get; set; }
        /// <summary> Элементы заказа </summary>
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
