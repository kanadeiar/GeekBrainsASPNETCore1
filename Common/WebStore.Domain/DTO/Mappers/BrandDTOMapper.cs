using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mappers
{
    /// <summary> Маппер брендов </summary>
    public static class BrandDTOMapper
    {
        /// <summary> В дтошку </summary>
        public static BrandDTO ToDTO(this Brand brand)
        {
            return brand is null
                ? null
                : new BrandDTO
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Order = brand.Order,
                    Timestamp = brand.Timestamp,
                    ProductsIds = brand.Products.Select(p => p.Id),
                };
        }
        /// <summary> Из дтошки </summary>
        public static Brand FromDTO(this BrandDTO brand)
        {
            return brand is null
                ? null
                : new Brand
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Order = brand.Order,
                    Timestamp = brand.Timestamp,
                    Products = brand.ProductsIds.Select(i => new Product{Id = i}).ToList(),
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> brands) => brands.Select(ToDTO);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> brands) => brands.Select(FromDTO);
    }
}
