using System.Collections.Generic;
using WebStore.Domain.WebModels.Admin;
using WebStore.Domain.WebModels.Shared;

namespace WebStore.Domain.WebModels.Product
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
