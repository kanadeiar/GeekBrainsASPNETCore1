using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.DTO
{
    public class ProductPageDTO
    {
        /// <summary> Отобранные товары </summary>
        public IEnumerable<ProductDTO> Products { get; set; }
        /// <summary> Всего товаров вообще </summary>
        public int TotalCount { get; set; }
    }
}
