using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Shared;
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        #region Тестирование списка товаров

        [TestMethod]
        public void Index_SendSection1Request_ShouldCorrectView()
        {
            const int expectedCountProducts = 3;
            const int expectedIdFirst = 1;
            const string expectedNameFirst = "Товар 1";
            const decimal expectedPriceFirst = 100;
            const int expectedSection = 1;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), false).Result)
                .Returns<ProductFilter, bool>((_, _) => Enumerable.Range(1, expectedCountProducts)
                    .Select(id => new Product
                    {
                        Id = id,
                        Name = $"Товар {id}",
                        Order = id,
                        Price = expectedPriceFirst,
                        ImageUrl = $"Image_{id}.jpg",
                    })
                );
            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Index(null, 1);

            Assert
                .IsInstanceOfType(result.Result, typeof(ViewResult));
            var viewResult =  (ViewResult) result.Result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(CatalogWebModel));
            var catalogWebModel = (CatalogWebModel) viewResult.Model;
            Assert
                .IsNull(catalogWebModel.BrandId);
            Assert
                .AreEqual(expectedSection, catalogWebModel.SectionId);
            Assert
                .IsInstanceOfType(catalogWebModel.PageWebModel, typeof(PageWebModel));
            Assert
                .AreEqual(1, catalogWebModel.PageWebModel.PageNumber);
            Assert
                .AreEqual(1, catalogWebModel.PageWebModel.TotalPages);
            Assert
                .AreEqual(expectedCountProducts, catalogWebModel.Products.Count());
            var firstWebModel = catalogWebModel.Products.First();
            Assert
                .AreEqual(expectedIdFirst, firstWebModel.Id);
            Assert
                .AreEqual(expectedNameFirst, firstWebModel.Name);
            Assert
                .AreEqual(expectedPriceFirst, firstWebModel.Price);
            Assert
                .IsNull(firstWebModel.Brand);
            Assert
                .IsNull(firstWebModel.Section);
            productDataMock
                .Verify(_ => _.GetProducts(It.IsAny<ProductFilter>(), false).Result, Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        #endregion
        
        #region Тестирование детального отображения товара

        [TestMethod]
        public void Details_SendRequest_ShouldCorrectView()
        {
            const int expectedId = 1;
            const string expectedName = "Товар 1";
            const decimal expectedPrice = 10m;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((int id) => new Product
                {
                    Id = id,
                    Name = $"Товар {id}",
                    Order = id,
                    Price = expectedPrice,
                    ImageUrl = $"Image_{id}.jpg",
                    BrandId = 1,
                    Brand = new Brand { Id = 1, Name = "Бренд_1", Order = 1 },
                    SectionId = 1,
                    Section = new Section { Id = 1, Name = "Категория 1", Order = 1 }
                });
            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Details(expectedId);
            
            Assert
                .IsInstanceOfType(result.Result, typeof(ViewResult));
            var viewResult = (ViewResult) result.Result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(ProductWebModel));
            var webModel = (ProductWebModel) viewResult.Model;
            Assert
                .AreEqual(expectedId, webModel.Id);
            Assert
                .AreEqual(expectedName, webModel.Name);
            Assert
                .AreEqual(expectedPrice, webModel.Price);
            productDataMock
                .Verify(_ => _.GetProductById(It.IsAny<int>()), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Details_SendNullRequest_ShouldNotFound()
        {
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((Product) null);
            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Details(1);

            Assert
                .IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion
    }
}
