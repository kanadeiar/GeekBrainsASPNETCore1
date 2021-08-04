using System.Collections.Generic;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    /// <summary> Ключевое слово для товара </summary>
    public class Tag : Entity
    {
        /// <summary> Само ключевое слово </summary>
        public string Text { get; set; }
        /// <summary> Товары с этим ключевым словом </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
