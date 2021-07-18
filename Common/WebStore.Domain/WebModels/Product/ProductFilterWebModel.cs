namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Веб-модель фильрации товаров по названию </summary>
    public class ProductFilterWebModel
    {
        /// <summary> Название товара </summary>
        public string Name { get; set; }
        /// <summary> Конструктор </summary>
        /// <param name="name">Название товара</param>
        public ProductFilterWebModel(string name)
        {
            Name = name;
        }
    }
}
