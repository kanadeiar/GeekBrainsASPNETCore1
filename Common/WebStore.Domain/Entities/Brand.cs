using System.Collections.Generic;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary> Бренд товара </summary>
    public class Brand : NamedEntity, IOrderedEntity
    {
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Товары </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
