using WebStore.Domain.Models.Interfaces;

namespace WebStore.Domain.Models
{
    /// <summary> Фильтр товаров </summary>
    public class ProductFilter : IProductFilter
    {
        /// <summary> Категория </summary>
        public int? SectionId { get; set; }
        /// <summary> Бренд </summary>
        public int? BrandId { get; set; }
        /// <summary> Идентификаторы </summary>
        public int[] Ids { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
    }
}
