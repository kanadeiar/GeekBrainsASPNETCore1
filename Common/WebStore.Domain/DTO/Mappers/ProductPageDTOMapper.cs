using WebStore.Domain.Models;

namespace WebStore.Domain.DTO.Mappers
{
    /// <summary> Маппер пагинации товаров </summary>
    public static class ProductPageDTOMapper
    {
        /// <summary> В дтошку </summary>
        /// <param name="Page">Модель</param>
        public static ProductPageDTO ToDTO(this ProductPage Page) => new ProductPageDTO {Products = Page.Products.ToDTO(), TotalCount = Page.TotalCount};
        /// <summary> Из дтошки </summary>
        /// <param name="Page">Дтошка</param>
        public static ProductPage FromDTO(this ProductPageDTO Page) => new ProductPage {Products = Page.Products.FromDTO(), TotalCount = Page.TotalCount};
    }
}
