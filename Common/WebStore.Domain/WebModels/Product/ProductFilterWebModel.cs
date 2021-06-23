namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Веб-модель фильрации товаров по названию </summary>
    public class ProductFilterWebModel
    {
        public string Name { get; set; }
        public ProductFilterWebModel(string name)
        {
            Name = name;
        }
    }
}
