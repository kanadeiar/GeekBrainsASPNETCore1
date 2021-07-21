using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels.Product;

namespace WebStore.Domain.Tests.WebModels.Product
{
    [TestClass]
    public class ProductSortWebModelTests
    {
        [TestMethod]
        public void CreateFirst_Created_Correct()
        {
            const ProductSortState expectedPrevious = ProductSortState.NameAsc;
            const ProductSortState expectedCurrent = ProductSortState.NameDesc;
            const bool expectedUp = true;

            var model = new ProductSortWebModel(default);

            Assert
                .IsInstanceOfType(model, typeof(ProductSortWebModel));
            Assert
                .AreEqual(expectedPrevious, model.Previous);
            Assert
                .AreEqual(expectedCurrent, model.Current);
            Assert
                .AreEqual(expectedUp, model.Up);
            Assert
                .AreEqual(expectedCurrent, model.NameSort);
            Assert
                .AreEqual(ProductSortState.OrderAsc, model.OrderSort);
            Assert
                .AreEqual(ProductSortState.SectionAsc, model.SectionSort);
            Assert
                .AreEqual(ProductSortState.BrandAsc, model.BrandSort);
            Assert
                .AreEqual(ProductSortState.PriceAsc, model.PriceSort);
        }
    }
}
