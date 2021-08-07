using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Mappers
{
    /// <summary> Маппер изображений </summary>
    public static class ImageUrlDTOMapper
    {
        /// <summary> В дтошку </summary>
        public static ImageUrlDTO ToDTO(this ImageUrl image)
        {
            return image is null
                ? null
                : new ImageUrlDTO
                {
                    Id = image.Id,
                    Url = image.Url,
                };
        }
        /// <summary> Из дтошки </summary>
        public static ImageUrl FromDTO(this ImageUrlDTO image)
        {
            return image is null
                ? null
                : new ImageUrl
                {
                    Id = image.Id,
                    Url = image.Url,
                };
        }
        /// <summary> В дтошку </summary>
        public static IEnumerable<ImageUrlDTO> ToDTO(this IEnumerable<ImageUrl> images) => images.Select(ToDTO);
        /// <summary> Из дтошки </summary>
        public static IEnumerable<ImageUrl> FromDTO(this IEnumerable<ImageUrlDTO> images) => images.Select(FromDTO);
    }
}
