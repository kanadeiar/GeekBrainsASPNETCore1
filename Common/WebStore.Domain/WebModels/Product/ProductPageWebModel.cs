using System.Collections.Generic;
using WebStore.Domain.WebModels.Shared;

namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Общая вебмодель для отображения отфильтрованных продуктов </summary>
    public class ProductPageWebModel
    {
        /// <summary> Фильтр товаров </summary>
        public ProductFilterWebModel Filter { get; set; }
        /// <summary> Сортировка товаров </summary>
        public ProductSortWebModel Sort { get; set; }
        /// <summary> Пагинация товаров </summary>
        public PageWebModel Page { get; set; }
        /// <summary> Товары </summary>
        public IEnumerable<EditProductWebModel> Products { get; set; }
    }
}
