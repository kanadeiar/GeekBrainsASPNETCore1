using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Tests.Entities
{
    [TestClass]
    public class CartTests
    {
        private Cart _cart;

        [TestInitialize]
        public void Initialize()
        {
            _cart = new Cart
            {
                Items = Enumerable.Range(1, 5).Select(i => new CartItem()
                {
                    ProductId = i,
                    Quantity = i,
                }).ToList()
            };
        }

        [TestMethod]
        public void ItemsCount_5CartItem_Should5()
        {
            var cart = _cart;
            const int expectedCount = 15;

            var actualCount = cart.ItemsSum;

            Assert
                .AreEqual(expectedCount, actualCount);
        }
    }
}
