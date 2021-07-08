using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Domain.DTO.Mappers.Order
{
    public static class OrderItemDTOMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem item)
        {
            return item is null
                ? null
                : new OrderItemDTO
                {
                    Id = item.Id,
                    ProductId = item.Product.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
        }

        public static OrderItem FromDTO(this OrderItemDTO item)
        {
            return item is null
                ? null
                : new OrderItem
                {
                    Id = item.Id,
                    Product = new Product { Id = item.ProductId },
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
        }
        public static IEnumerable<OrderItemDTO> ToDTO(this CartWebModel cart)
        {
            return cart.Items.Select(p => new OrderItemDTO
            {
                Id = p.Product.Id,
                Price = p.Product.Price,
                Quantity = p.Quantity,
            });
        }
        public static CartWebModel FromDTO(this IEnumerable<OrderItemDTO> items)
        {
            return new CartWebModel
            {
                Items = items.Select(p => (new ProductWebModel{Id = p.ProductId}, p.Quantity, p.Price * p.Quantity)),
            };
        }
    }
}
