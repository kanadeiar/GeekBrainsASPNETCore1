namespace WebStore.Domain.DTO
{
    /// <summary> Бренд </summary>
    public class BrandDTO
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Штамп временной </summary>
        public byte[] Timestamp { get; set; }
    }
}
