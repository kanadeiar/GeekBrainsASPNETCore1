using System.Collections.Generic;

namespace WebStore.Domain.Models
{
    /// <summary> Список желаний товаров </summary>
    public class Wanted
    {
        /// <summary> Список товаров - желаний </summary>
        public ICollection<int> ProductsIds { get; set; } = new List<int>();
    }
}
