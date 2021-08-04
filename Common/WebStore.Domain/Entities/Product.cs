using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Domain.Entities
{
    /// <summary> Товар магазина </summary>
    public class Product : NamedEntity, IOrderedEntity
    {
        /// <summary> Сотрировка </summary>
        public int Order { get; set; }
        /// <summary> Категория </summary>
        public int SectionId { get; set; }
        /// <summary> Категория </summary>
        [ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }
        /// <summary> Бренд </summary>
        public int? BrandId { get; set; }
        /// <summary> Бренд </summary>
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }
        /// <summary> Изображение </summary>
        public string ImageUrl { get; set; }
        /// <summary> Стоимость </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        /// <summary> Товар удален в корзину </summary>
        public bool IsDelete { get; set; }
        /// <summary> Ключевые слова этого товара </summary>
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        /// <summary> Элементы заказа </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
