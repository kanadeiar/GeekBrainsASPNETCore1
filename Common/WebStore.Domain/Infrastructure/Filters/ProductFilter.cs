using WebStore.Domain.Infrastructure.Interfaces;

namespace WebStore.Domain.Infrastructure.Filters
{
    public class ProductFilter : IProductFilter
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public int[] Ids { get; set; }
        public string Name { get; set; }
    }
}
