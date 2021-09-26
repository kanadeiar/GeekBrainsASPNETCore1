using System.Collections.Generic;

namespace WebStore.Domain.Models
{
    /// <summary> Сравнение товаров </summary>
    public class Compare
    {
        /// <summary> Список товаров - желаний </summary>
        public ICollection<int> ProductsIds { get; set; } = new List<int>();
    }
}
