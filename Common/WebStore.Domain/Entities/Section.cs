using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary> Категория товара </summary>
    public class Section : NamedEntity, IOrderedEntity
    {
        /// <summary> Заказ </summary>
        public int Order { get; set; }
        /// <summary> Родительский элемент </summary>
        public int? ParentId { get; set; }
        /// <summary> Родительский элемент </summary>
        [ForeignKey(nameof(ParentId))]
        public Section Parent { get; set; }
        /// <summary> Товары этой категории </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
