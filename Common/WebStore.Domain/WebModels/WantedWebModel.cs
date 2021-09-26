using System.Collections.Generic;
using WebStore.Domain.WebModels.Product;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель списка желаемых товаров </summary>
    public class WantedWebModel
    {
        /// <summary> Желаемые товары </summary>
        public IEnumerable<ProductWebModel> Items { get; set; }
    }
}
