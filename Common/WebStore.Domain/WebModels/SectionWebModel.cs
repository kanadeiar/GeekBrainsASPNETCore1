using System.Collections.Generic;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель категории </summary>
    public class SectionWebModel
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
        /// <summary> Сортировка </summary>
        public int Order { get; set; }
        /// <summary> Родительский элемент </summary>
        public SectionWebModel Parent { get; set; }
        /// <summary> Детки </summary>
        public List<SectionWebModel> Children { get; set; } = new List<SectionWebModel>();
        /// <summary> Количество товаров </summary>
        public int CountProduct { get; set; }
    }
}
