using System.Collections.Generic;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class ImageUrl : Entity
    {
        /// <summary> Изображение </summary>
        public string Url { get; set; }
        /// <summary> Товары с этим изображением </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
