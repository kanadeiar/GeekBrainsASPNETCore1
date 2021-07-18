using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities.Orders
{
    /// <summary> Товар заказа </summary>
    public class OrderItem : Entity
    {
        /// <summary> Заказ </summary>
        [Required]
        public Order Order { get; set; }
        /// <summary> Товар </summary>
        [Required]
        public Product Product { get; set; }
        /// <summary> Стоимость </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        /// <summary> Количество </summary>
        public int Quantity { get; set; }
        /// <summary> Стоимость суммарная </summary>
        [NotMapped] public decimal SumPrice => Price * Quantity;
    }
}