namespace WebStore.Domain.Models.Interfaces
{
    /// <summary> Фильтр по товарам </summary>
    public interface IProductFilter
    {
        /// <summary> Категория </summary>
        int? SectionId { get; set; }
        /// <summary> Бренд </summary>
        int? BrandId { get; set; }
        /// <summary> Идентификаторы </summary>
        int[] Ids { get; set; }
        /// <summary> Фильтр по названию товара </summary>
        string Name { get; set; }
    }
}