namespace WebStore.Domain.Models
{
    /// <summary> Состояние сортировки </summary>
    public enum ProductSortState
    {
        /// <summary> По названию по возврастанию </summary>
        NameAsc,
        /// <summary> По названию по убыванию </summary>
        NameDesc,
        /// <summary> По сортировке по возрастанию </summary>
        OrderAsc,
        /// <summary> По сортировке по убыванию </summary>
        OrderDesc,
        /// <summary> По категории по возрастанию </summary>
        SectionAsc,
        /// <summary> По категории по убыванию </summary>
        SectionDesc,
        /// <summary> По бренду по возрастанию </summary>
        BrandAsc,
        /// <summary> По бренду по убыванию </summary>
        BrandDesc,
        /// <summary> По стоимости по возрастанию </summary>
        PriceAsc,
        /// <summary> По стоимости по убыванию </summary>
        PriceDesc,
    }
}
