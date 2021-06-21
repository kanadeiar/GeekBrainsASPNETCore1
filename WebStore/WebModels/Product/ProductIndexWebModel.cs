using System.Collections.Generic;

namespace WebStore.WebModels.Product
{
    /// <summary> Общая вебмодель для отображения отфильтрованных продуктов </summary>
    public class ProductIndexWebModel
    {
        public ProductSortWebModel Sort { get; set; }
        public IEnumerable<ProductEditWebModel> Products { get; set; }
    }
}
