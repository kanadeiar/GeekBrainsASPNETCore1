using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mappers
{
    /// <summary> Маппер товаров </summary>
    public static class ProductDTOMapper
    {
        /// <summary> В дтошку </summary>
        public static ProductDTO ToDTO(this Product product)
        {
            return product is null
                ? null
                : new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    Brand = product.Brand.ToDTO(),
                    Section = product.Section.ToDTO(),
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Timestamp = product.Timestamp,
                };
        }
        /// <summary> Из дтошки </summary>
        public static Product FromDTO(this ProductDTO product)
        {
            return product is null
                ? null
                : new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    Brand = product.Brand.FromDTO(),
                    Section = product.Section.FromDTO(),
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Timestamp = product.Timestamp,
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> products) => products.Select(ToDTO);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> products) => products.Select(FromDTO);
    }
}
