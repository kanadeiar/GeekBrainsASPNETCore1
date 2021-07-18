using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Order;

namespace WebStore.Domain.DTO.Mappers.Order
{
    /// <summary> Маппер для заказов </summary>
    public static class OrderDTOMapper
    {
        /// <summary> В дтошку </summary>
        public static OrderDTO ToDTO(this Entities.Orders.Order order)
        {
            return order is null
                ? null
                : new OrderDTO
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    DateTime = order.DateTime,
                    Items = order.Items.Select(OrderItemDTOMapper.ToDTO),
                };
        }
        /// <summary> Из дтошки </summary>
        public static Entities.Orders.Order FromDTO(this OrderDTO order)
        {
            return order is null
                ? null
                : new Entities.Orders.Order
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    DateTime = order.DateTime,
                    Items = order.Items.Select(OrderItemDTOMapper.FromDTO).ToList(),
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Entities.Orders.Order> orders) =>
            orders.Select(ToDTO);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<Entities.Orders.Order> FromDTO(this IEnumerable<OrderDTO> orders) =>
            orders.Select(FromDTO);
    }
}
