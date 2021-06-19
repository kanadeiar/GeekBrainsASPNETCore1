using System.Collections.Generic;
using System.Linq;

namespace WebStore.WebModels.Cart
{
    public class CartWebModel
    {
        public IEnumerable<(ProductWebModel Product, int Quantity, decimal PriceSum)> Items { get; set; }
        public int ItemsSum => Items?.Sum(p => p.Quantity) ?? 0;
        public decimal PriceSum => Items?.Sum(p => p.Product.Price * p.Quantity) ?? 0;
    }
}
