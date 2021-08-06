using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Product;
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
            const int expectedPage = 1;
            const int expectedTotalPages = 1;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), false))
                .ReturnsAsync((ProductFilter _, bool _) => 
                    new ProductPage
                    {
                        Products = Enumerable.Range(1, expectedCountProducts)
                            .Select(id => new Product
                            {
                                Id = id,
                                Name = $"Товар {id}",
                                Order = id,
                                SectionId = id,
                                Section = new Section{Id = id},
                                BrandId = id,
                                Brand = new Brand{Id = id},
                                Price = expectedPriceFirst,
                                ImageUrl = $"Image_{id}.jpg",
                            }),
                        TotalCount = expectedCountProducts,
                    }
                );
            var configurationStub = Mock
                .Of<IConfiguration>();
            var controller = new CatalogController(productDataMock.Object, configurationStub);

            var result = controller.Index(null, 1).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult =  (ViewResult) result;
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
                .AreEqual(expectedPage, catalogWebModel.PageWebModel.Page);
            Assert
                .AreEqual(expectedTotalPages, catalogWebModel.PageWebModel.TotalPages);
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
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), It.IsAny<bool>()))
                .ReturnsAsync((ProductFilter _, bool _) =>
                {
                    return new ProductPage
                    {
                        Products = Enumerable.Range(1, 3)
                            .Select(id => new Product
                            {
                                Id = id,
                                Name = $"Товар {id}",
                                Order = id,
                                Price = 100,
                                ImageUrl = $"Image_{id}.jpg",
                            }),
                        TotalCount = 3,
                    };
                });
            var configurationStub = Mock
                .Of<IConfiguration>();
            var controller = new CatalogController(productDataMock.Object, configurationStub);

            var result = controller.Details(expectedId).Result;
            
            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
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
                .Verify();
        }

        [TestMethod]
        public void Details_SendNullRequest_ShouldNotFound()
        {
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((Product) null);
            var configurationStub = Mock
                .Of<IConfiguration>();
            var controller = new CatalogController(productDataMock.Object, configurationStub);

            var result = controller.Details(1);

            Assert
                .IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion

        #region Тестирование WebApi
        
        [TestMethod]
        public void ApiGetProductPartialView_SendRequest_ShouldPartialView()
        {
            const int expectedCountProducts = 3;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), false))
                .ReturnsAsync((ProductFilter _, bool _) =>
                    new ProductPage
                    {
                        Products = Enumerable.Range(1, expectedCountProducts)
                            .Select(id => new Product
                            {
                                Id = id,
                                Name = $"Товар {id}",
                                Order = id,
                                Price = 100,
                                ImageUrl = $"Image_{id}.jpg",
                            }),
                        TotalCount = expectedCountProducts,
                    }
                );
            var configurationStub = Mock
                .Of<IConfiguration>();
            var controller = new CatalogController(productDataMock.Object, configurationStub);

            var result = controller.ApiGetProductPartialView(null, null).Result;

            Assert
                .IsInstanceOfType(result, typeof(PartialViewResult));
            var partialView = (PartialViewResult)result;
            Assert
                .AreEqual("Partial/_ProductsPartial", partialView.ViewName);
        }

        [TestMethod]
        public void ApiGetCatalogPaginationPartialView_SendRequest_ShouldPartialView()
        {
            const int expectedCountProducts = 3;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), false))
                .ReturnsAsync((ProductFilter _, bool _) =>
                    new ProductPage
                    {
                        Products = Enumerable.Range(1, expectedCountProducts)
                            .Select(id => new Product
                            {
                                Id = id,
                                Name = $"Товар {id}",
                                Order = id,
                                Price = 100,
                                ImageUrl = $"Image_{id}.jpg",
                            }),
                        TotalCount = expectedCountProducts,
                    }
                );
            var configurationStub = Mock
                .Of<IConfiguration>();
            var controller = new CatalogController(productDataMock.Object, configurationStub);

            var result = controller.ApiGetCatalogPaginationPartialView(null, null).Result;

            Assert
                .IsInstanceOfType(result, typeof(PartialViewResult));
            var partialView = (PartialViewResult)result;
            Assert
                .AreEqual("Partial/_CatalogPaginationPartial", partialView.ViewName);
        }

        #endregion
    }
}
