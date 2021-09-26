using System.Collections.Generic;

namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Веб модель товара </summary>
    public class ProductWebModel
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
        /// <summary> Категория товара </summary>
        public string Section { get; set; }
        /// <summary> Бренд товара </summary>
        public string Brand { get; set; }
        /// <summary> Путь к картинке </summary>
        public string ImageUrl { get; set; }
        /// <summary> Второстепенные картинки </summary>
        public ICollection<string> ImageUrls { get; set; }
        /// <summary> Стоимость </summary>
        public decimal Price { get; set; }
        /// <summary> Ключевые слова этого товара </summary>
        public ICollection<TagWebModel> Tags { get; set; }
    }
}
