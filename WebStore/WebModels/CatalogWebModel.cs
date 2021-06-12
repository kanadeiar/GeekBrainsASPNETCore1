using System.Collections.Generic;

namespace WebStore.WebModels
{
    public class CatalogWebModel
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public IEnumerable<ProductWebModel> Products { get; set; }
    }
}
