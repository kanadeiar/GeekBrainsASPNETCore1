using System.Collections.Generic;

namespace WebStore.Domain.DTO
{
    /// <summary> Дтошка для пагинатора товаров </summary>
    public class ProductPageDTO
    {
        /// <summary> Отобранные товары </summary>
        public IEnumerable<ProductDTO> Products { get; set; }
        /// <summary> Всего товаров вообще </summary>
        public int TotalCount { get; set; }
    }
}
