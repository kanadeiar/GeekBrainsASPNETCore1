using System.Collections.Generic;

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
        /// <summary> Идентификаторы товаров этой категории </summary>
        public IEnumerable<int> ProductsIds { get; set; } = new List<int>();
        /// <summary> Штамп временной </summary>
        public byte[] Timestamp { get; set; }
    }
}
