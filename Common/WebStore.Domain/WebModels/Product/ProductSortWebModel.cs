using WebStore.Domain.Models;

namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Вебмодель сортировки продуктов </summary>
    public class ProductSortWebModel
    {
        public ProductSortState NameSort { get; set; } = ProductSortState.NameAsc;
        public ProductSortState OrderSort { get; set; } = ProductSortState.OrderAsc;
        public ProductSortState SectionSort { get; set; } = ProductSortState.SectionAsc;
        public ProductSortState BrandSort { get; set; } = ProductSortState.BrandAsc;
        public ProductSortState PriceSort { get; set; } = ProductSortState.PriceAsc;
        public ProductSortState Current { get; set; }
        public ProductSortState Previous { get; set; }
        public bool Up { get; set; } = true;

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
