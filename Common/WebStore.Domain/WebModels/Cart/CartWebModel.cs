using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.WebModels.Cart
{
    /// <summary> Веб модель корзины товаров </summary>
    public class CartWebModel
    {
        /// <summary> Товары корзины </summary>
        public IEnumerable<(ProductWebModel Product, int Quantity, decimal PriceSum)> Items { get; set; }
        /// <summary> Количество элементов в корзине </summary>
        public int ItemsSum => Items?.Sum(p => p.Quantity) ?? 0;
        /// <summary> Сумма стоимости товаров корзины </summary>
        public decimal PriceSum => Items?.Sum(p => p.Product.Price * p.Quantity) ?? 0;
    }
}
