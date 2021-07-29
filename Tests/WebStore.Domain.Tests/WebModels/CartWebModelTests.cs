using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Domain.Tests.WebModels
{
    [TestClass]
    public class CartWebModelTests
    {
        [TestMethod]
        public void CartWebModel_3Items_ShouldCorrectItemsSum()
        {
            const int expectedCount = 3;
            var cartWebModel = new CartWebModel
            {
                Items = new List<(ProductWebModel Product, int Quantity)>{
                    new (new ProductWebModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1),
                    new (new ProductWebModel { Id = 2, Name = "Product 2", Price = 1.5m }, 2)
                }
            };

            var actualCount = cartWebModel.ItemsSum;

            Assert
                .AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartWebModel_3Items_ShouldCorrectPriceSum()
        {
            var cartWebModel = new CartWebModel
            {
                Items = new List<(ProductWebModel Product, int Quantity, decimal Sum)>{
                    new (new ProductWebModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1, 0.5m),
                    new (new ProductWebModel { Id = 2, Name = "Product 2", Price = 1.5m }, 2, 3m)
                }
            };
            decimal expectedTotalPrice = cartWebModel.Items.Sum(i => i.Quantity * i.Product.Price);

            var actualTotalPrice = cartWebModel.PriceSum;

            Assert
                .AreEqual(expectedTotalPrice, actualTotalPrice);
        }
    }
}
