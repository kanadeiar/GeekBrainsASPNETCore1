using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Models
{
    /// <summary> Для пагинации </summary>
    public class ProductPage
    {
        /// <summary> Отобранные товары </summary>
        public IEnumerable<Product> Products { get; set; }
        /// <summary> Всего товаров вообще </summary>
        public int TotalCount { get; set; }
    }
}
