namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель товара </summary>
    public class ProductWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
