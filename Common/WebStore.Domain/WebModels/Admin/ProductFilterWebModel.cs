namespace WebStore.Domain.WebModels.Admin
{
    public class ProductFilterWebModel
    {
        public string Name { get; set; }
        public ProductFilterWebModel(string name)
        {
            Name = name;
        }
    }
}
