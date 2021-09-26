using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.WebModels.Product;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель сравнения товаров </summary>
    public class CompareWebModel
    {
        /// <summary> Сравниваемые товары </summary>
        public IEnumerable<ProductWebModel> Items { get; set; }
        /// <summary> Возвращение на страницу </summary>
        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
