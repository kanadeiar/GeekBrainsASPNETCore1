namespace WebStore.Areas.Admin.WebModels
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
