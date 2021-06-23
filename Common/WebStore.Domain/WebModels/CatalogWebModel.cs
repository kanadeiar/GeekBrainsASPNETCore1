using System.Collections.Generic;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель категории </summary>
    public class CatalogWebModel
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public IEnumerable<ProductWebModel> Products { get; set; }
    }
}
