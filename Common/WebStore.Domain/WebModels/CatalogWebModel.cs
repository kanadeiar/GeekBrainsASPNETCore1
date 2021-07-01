using System.Collections.Generic;
using WebStore.Domain.WebModels.Shared;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель категории </summary>
    public class CatalogWebModel
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public PageWebModel PageWebModel { get; set; }
        public IEnumerable<ProductWebModel> Products { get; set; }
    }
}
