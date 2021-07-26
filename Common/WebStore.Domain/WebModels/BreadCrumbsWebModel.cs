using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель отображения хлебных крошек </summary>
    public class BreadCrumbsWebModel
    {
        /// <summary> Категория товара </summary>
        public Section Section { get; set; }
        /// <summary> Бренд </summary>
        public Brand Brand { get; set; }
        /// <summary> Название товара </summary>
        public string Product { get; set; }
    }
}
