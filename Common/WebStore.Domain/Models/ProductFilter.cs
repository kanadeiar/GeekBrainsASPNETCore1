using WebStore.Domain.Models.Interfaces;

namespace WebStore.Domain.Models
{
    public class ProductFilter : IProductFilter
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public int[] Ids { get; set; }
        public string Name { get; set; }
    }
}
