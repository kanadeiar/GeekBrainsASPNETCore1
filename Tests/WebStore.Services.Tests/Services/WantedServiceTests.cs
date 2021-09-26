using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class WantedServiceTests
    {
        [TestMethod]
        public void Add_Id1_ShouldCorrect()
        {
            const int expectedId = 9;
            const int expectedCount = 6;
            var productDataStub = Mock
                .Of<IProductData>();
            var wantedStore = new Mock<IWantedStore>();
            var wanted = new Wanted
            {
                ProductsIds = Enumerable.Range(1, expectedCount - 1).Select(i => i).ToList(),
            };
            wantedStore
                .Setup(_ => _.Wanted)
                .Returns(wanted);
            var wantedService = new WantedService(wantedStore.Object, productDataStub);

            wantedService.Add(expectedId);

            Assert
                .AreEqual(expectedCount, wanted.ProductsIds.Count);
            Assert
                .IsTrue(wanted.ProductsIds.Contains(expectedId));
        }

        [TestMethod]
        public void Remove_Id1_ShouldCorrect()
        {
            const int targetId = 1;
            const int expectedCount = 4;
            var productDataStub = Mock
                .Of<IProductData>();
            var wantedStore = new Mock<IWantedStore>();
            var wanted = new Wanted
            {
                ProductsIds = Enumerable.Range(1, expectedCount + 1).Select(i => i).ToList(),
            };
            wantedStore
                .Setup(_ => _.Wanted)
                .Returns(wanted);
            var wantedService = new WantedService(wantedStore.Object, productDataStub);

            wantedService.Remove(targetId);

            Assert
                .AreEqual(expectedCount, wanted.ProductsIds.Count);
            Assert
                .IsFalse(wanted.ProductsIds.Contains(targetId));
        }

        [TestMethod]
        public void Clear_All_ShouldCorrect()
        {
            const int expectedCount = 0;
            var productDataStub = Mock
                .Of<IProductData>();
            var wantedStore = new Mock<IWantedStore>();
            var wanted = new Wanted
            {
                ProductsIds = Enumerable.Range(1, expectedCount).Select(i => i).ToList(),
            };
            wantedStore
                .Setup(_ => _.Wanted)
                .Returns(wanted);
            var wantedService = new WantedService(wantedStore.Object, productDataStub);

            wantedService.Clear();

            Assert
                .AreEqual(expectedCount, wanted.ProductsIds.Count);
        }

        [TestMethod]
        public void GetWebModel_5Items_ShouldWebModel()
        {
            const int expectedCount = 5;
            const decimal expectedPrice = 1.1m;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), false).Result)
                .Returns(new ProductPage
                {
                    Products = Enumerable.Range(1, expectedCount).Select(i => new Product
                    {
                        Id = i,
                        Name = $"Product {i}",
                        Price = 1.1m * i,
                        Order = i,
                        ImageUrl = $"image_{i}.jpg",
                        BrandId = i,
                        Brand = new Brand { Id = i, Name = $"Тестовый бренд {i}", Order = i },
                        SectionId = i,
                        Section = new Section { Id = i, Name = $"Категория {i}", Order = i },
                    }),
                    TotalCount = 5,
                });
            var wantedStore = new Mock<IWantedStore>();
            var wanted = new Wanted
            {
                ProductsIds = Enumerable.Range(1, expectedCount).Select(i => i).ToList(),
            };
            wantedStore
                .Setup(_ => _.Wanted)
                .Returns(wanted);
            var wantedService = new WantedService(wantedStore.Object, productDataMock.Object);

            var result = wantedService.GetWebModel().Result;

            Assert
                .IsInstanceOfType(result, typeof(WantedWebModel));
            Assert
                .AreEqual(expectedCount, result.Items.Count());
            Assert
                .AreEqual(expectedPrice, result.Items.First().Price);
        }
    }
}
