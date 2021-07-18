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
        public void ItemsCount_Returns_Correct()
        {
            var cart = _cart;
            var expectedCount = _cart.Items.Sum(i => i.Quantity);

            var actualCount = cart.ItemsSum;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
