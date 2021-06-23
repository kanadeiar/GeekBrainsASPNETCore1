using System.Collections.Generic;
using WebStore.Areas.Admin.WebModels;
using WebStore.WebModels.Shared;

namespace WebStore.WebModels.Product
{
    /// <summary> Общая вебмодель для отображения отфильтрованных продуктов </summary>
    public class ProductIndexWebModel
    {
        public ProductFilterWebModel Filter { get; set; }
        public ProductSortWebModel Sort { get; set; }
        public PageWebModel Page { get; set; }
        public IEnumerable<ProductEditWebModel> Products { get; set; }
    }
}
