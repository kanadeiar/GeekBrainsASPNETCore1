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
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> brands) => brands.Select(ToDTO);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> brands) => brands.Select(FromDTO);
    }
}
