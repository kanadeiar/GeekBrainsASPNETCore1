using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.Entities
{
    public class Cart
    {
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public int ItemsSum => Items?.Sum(i => i.Quantity) ?? 0;
    }
}
