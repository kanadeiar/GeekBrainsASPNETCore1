using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель отображения хлебных крошек </summary>
    public class BreadCrumbsWebModel
    {
        /// <summary> Название контроллера в коде </summary>
        public string Controller { get; set; }
        /// <summary> Название контроллера обыкновенное </summary>
        public string ControllerText { get; set; }
        /// <summary> Действие контроллера </summary>
        public string ControllerAction { get; set; }
        /// <summary> Категория товара </summary>
        public Section Section { get; set; }
        /// <summary> Бренд </summary>
        public Brand Brand { get; set; }
        /// <summary> Название товара </summary>
        public string Product { get; set; }
    }
}
