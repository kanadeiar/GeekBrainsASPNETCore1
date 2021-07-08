namespace WebStore.Domain.DTO
{
    /// <summary> Товар </summary>
    public class ProductDTO
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название товара </summary>
        public string Name { get; set; }
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Категория товара </summary>
        public SectionDTO Section { get; set; }
        /// <summary> Бренд товара </summary>
        public BrandDTO Brand { get; set; }
        /// <summary> Путь к файлу с картинкой </summary>
        public string ImageUrl { get; set; }
        /// <summary> Стоимость товара </summary>
        public decimal Price { get; set; }
        /// <summary> Временной штамп </summary>
        public byte[] Timestamp { get; set; }
    }
}
