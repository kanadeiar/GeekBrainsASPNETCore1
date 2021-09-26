using System.Collections.Generic;
using WebStore.Domain.WebModels.Product;
using WebStore.Domain.WebModels.Shared;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель категории </summary>
    public class CatalogWebModel
    {
        /// <summary> Идентификатор </summary>
        public int? SectionId { get; set; }
        /// <summary> Бренд </summary>
        public int? BrandId { get; set; }
        /// <summary> Модель пагинации товаров </summary>
        public PageWebModel PageWebModel { get; set; }
        /// <summary> Товары </summary>
        public IEnumerable<ProductWebModel> Products { get; set; }
    }
}
