using System.Collections.Generic;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Domain.DTO.Order
{
    public class CreateOrderDTO
    {
        public CreateOrderWebModel Order { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
