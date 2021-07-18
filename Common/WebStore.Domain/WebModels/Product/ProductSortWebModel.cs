using WebStore.Domain.Models;

namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Вебмодель сортировки продуктов </summary>
    public class ProductSortWebModel
    {
        /// <summary> Сортировка по названию </summary>
        public ProductSortState NameSort { get; set; } = ProductSortState.NameAsc;
        /// <summary> Сортировка по номеру </summary>
        public ProductSortState OrderSort { get; set; } = ProductSortState.OrderAsc;
        /// <summary> Сортировка по категории </summary>
        public ProductSortState SectionSort { get; set; } = ProductSortState.SectionAsc;
        /// <summary> Сортировка по бренду </summary>
        public ProductSortState BrandSort { get; set; } = ProductSortState.BrandAsc;
        /// <summary> Сортировка по стоимости </summary>
        public ProductSortState PriceSort { get; set; } = ProductSortState.PriceAsc;
        /// <summary> Текущий статус сортировки, устанавливаемый </summary>
        public ProductSortState Current { get; set; }
        /// <summary> Прошлый статус сортировки, уже установленный </summary>
        public ProductSortState Previous { get; set; }
        /// <summary> Сотрировка по убыванию </summary>
        public bool Up { get; set; } = true;

        /// <summary> Конструктор </summary>
        /// <param name="sortOrder">Состояние сортировки</param>
        public ProductSortWebModel(ProductSortState sortOrder)
        {
            if (sortOrder == ProductSortState.NameDesc || sortOrder == ProductSortState.OrderDesc 
                || sortOrder == ProductSortState.PriceDesc || sortOrder == ProductSortState.SectionDesc 
                || sortOrder == ProductSortState.BrandDesc) 
                Up = false;

            Previous = sortOrder;

            switch (sortOrder)
            {
                case ProductSortState.NameDesc:
                    Current = NameSort = ProductSortState.NameAsc;
                    break;
                case ProductSortState.OrderAsc:
                    Current = OrderSort = ProductSortState.OrderDesc;
                    break;
                case ProductSortState.OrderDesc:
                    Current = OrderSort = ProductSortState.OrderAsc;
                    break;
                case ProductSortState.PriceAsc:
                    Current = PriceSort = ProductSortState.PriceDesc;
                    break;
                case ProductSortState.PriceDesc:
                    Current = PriceSort = ProductSortState.PriceAsc;
                    break;
                case ProductSortState.SectionAsc:
                    Current = SectionSort = ProductSortState.SectionDesc;
                    break;
                case ProductSortState.SectionDesc:
                    Current = SectionSort = ProductSortState.SectionAsc;
                    break;
                case ProductSortState.BrandAsc:
                    Current = BrandSort = ProductSortState.BrandDesc;
                    break;
                case ProductSortState.BrandDesc:
                    Current = BrandSort = ProductSortState.BrandAsc;
                    break;
                default:
                    Current = NameSort = ProductSortState.NameDesc;
                    break;
            }
        }
    }
}
