using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mappers
{
    public static class TagDTOMapper
    {
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
        public static IEnumerable<TagDTO> ToDto(this IEnumerable<Tag> keywords) => keywords.Select(ToDto);
        public static IEnumerable<Tag> FromDto(this IEnumerable<TagDTO> keywords) => keywords.Select(FromDto);
    }
}
