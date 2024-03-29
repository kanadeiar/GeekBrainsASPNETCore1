﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mappers
{
    /// <summary> Маппер категорий товаров </summary>
    public static class SectionDTOMapper
    {
        /// <summary> В дтошку </summary>
        public static SectionDTO ToDTO(this Section section)
        {
            return section is null
                ? null
                : new SectionDTO
                {
                    Id = section.Id,
                    Name = section.Name,
                    Order = section.Order,
                    ParentId = section.ParentId,
                    Timestamp = section.Timestamp,
                    ProductsIds = section.Products.Select(p => p.Id),
                };
        }
        /// <summary> Из дтошки </summary>
        public static Section FromDTO(this SectionDTO section)
        {
            return section is null
                ? null
                : new Section
                {
                    Id = section.Id,
                    Name = section.Name,
                    Order = section.Order,
                    ParentId = section.ParentId,
                    Timestamp = section.Timestamp,
                    Products = section.ProductsIds.Select(i => new Product{Id = i}).ToList(),
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> sections) => sections.Select(ToDTO);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO> sections) => sections.Select(FromDTO);
    }
}
