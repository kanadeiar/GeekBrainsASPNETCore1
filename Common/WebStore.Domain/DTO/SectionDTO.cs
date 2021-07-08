namespace WebStore.Domain.DTO
{
    /// <summary> Категория товара </summary>
    public class SectionDTO
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название категории </summary>
        public string Name { get; set; }
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Идентификатор родительского элемента </summary>
        public int? ParentId { get; set; }
        /// <summary> Временной штамп </summary>
        public byte[] Timestamp { get; set; }
    }
}
