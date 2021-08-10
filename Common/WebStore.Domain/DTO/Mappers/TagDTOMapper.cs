using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mappers
{
    /// <summary> Маппер дтошки ключевых слов </summary>
    public static class TagDTOMapper
    {
        /// <summary> В дтошку </summary>
        public static TagDTO ToDto(this Tag tag)
        {
            return tag is null
                ? null
                : new TagDTO
                {
                    Id = tag.Id,
                    Text= tag.Text,
                };
        }
        /// <summary> Из дтошки </summary>
        public static Tag FromDto(this TagDTO tag)
        {
            return tag is null
                ? null
                : new Tag
                {
                    Id = tag.Id,
                    Text = tag.Text,
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<TagDTO> ToDto(this IEnumerable<Tag> keywords) => keywords.Select(ToDto);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<Tag> FromDto(this IEnumerable<TagDTO> keywords) => keywords.Select(FromDto);
    }
}
