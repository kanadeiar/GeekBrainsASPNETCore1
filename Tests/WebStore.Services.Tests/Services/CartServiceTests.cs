using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;
        private ICartService _cartService;

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
            _productDataMock = new Mock<IProductData>();
            _productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>(), false).Result)
                .Returns(Enumerable.Range(1, 5).Select(i => new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Price = 1.1m * i,
                    Order = i,
                    ImageUrl = $"image_{i}.jpg",
                    BrandId = i,
                    Brand = new Brand{Id = i, Name = $"Тестовый бренд {i}", Order = i},
                    SectionId = i,
                    Section = new Section{Id = i, Name = $"Категория {i}", Order = i},
                }));
            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);
            var loggerStub = Mock
                .Of<ILogger<CartService>>();
            _cartService = new CartService(_cartStoreMock.Object, _productDataMock.Object, loggerStub);
        }

        [TestMethod]
        public void Add_Correct()
        {
            _cart.Items.Clear();
            const int expId = 1;
            const int expCount = 1;

            _cartService.Add(expId);

            Assert.AreEqual(expCount, _cart.ItemsSum);
            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expId, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void Remove_Correct()
        {
            const int removedId = 1;
            const int expFirstProductId = 2;
            const int expCount = 4;
            const int expItemsCount = 14;

            _cartService.Remove(removedId);

            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expItemsCount, _cart.ItemsSum);
            Assert.AreEqual(expFirstProductId, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void Clear_Correct()
        {
            _cartService.Clear();

            Assert.AreEqual(0, _cart.Items.Count);
        }

        [TestMethod]
        public void Subtract_Correct()
        {
            const int minusId = 2;
            const int expCountItem = 1;
            const int expCount = 5;
            const int expItemsCount = 14;

            _cartService.Subtract(minusId);

            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expItemsCount, _cart.ItemsSum);
            var items = _cart.Items.ToImmutableArray();
            Assert.AreEqual(minusId, items[1].ProductId);
            Assert.AreEqual(expCountItem, items[1].Quantity);
        }

        [TestMethod]
        public void RemoveItem_When_Decrement_to_0()
        {
            const int minusId = 1;
            const int expCount = 4;
            const int expItemsCount = 14;

            _cartService.Subtract(minusId);

            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expItemsCount, _cart.ItemsSum);
        }

        [TestMethod]
        public void GetWebModel_WorkCorrect()
        {
            const int expItemsCount = 15;
            const decimal expFirstProductPrice = 1.1m;

            var result = _cartService.GetWebModel();

            Assert.AreEqual(expItemsCount, result.Result.ItemsSum);
            Assert.AreEqual(expFirstProductPrice, result.Result.Items.First().Product.Price);
        }
    }
}
