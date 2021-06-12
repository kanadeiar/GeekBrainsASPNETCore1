using System.Collections.Generic;

namespace WebStore.ViewInterfaces
{
    public class CatalogInterface
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public IEnumerable<ProductInterface> Products { get; set; }
    }
}
