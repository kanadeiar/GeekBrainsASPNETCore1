using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.Entities
{
    /// <summary> Корзина покупки </summary>
    public class Cart
    {
        /// <summary> Товары в корзине </summary>
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        /// <summary> Сумма всех товаров в корзине </summary>
        public int ItemsSum => Items?.Sum(i => i.Quantity) ?? 0;
    }
}
