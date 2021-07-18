namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель бренда </summary>
    public class BrandWebModel
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
        /// <summary> Количество товаров </summary>
        public int CountProduct { get; set; }
    }
}
