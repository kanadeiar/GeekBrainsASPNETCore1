using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Controllers;

namespace WebStore.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ProductApiControllerTests
    {
        #region Тестирование веб апи контроллера товаров

        [TestMethod]
        public void GetSections_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestSection";
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetSections())
                .ReturnsAsync(() =>
                {
                    Task.Delay(1000).Wait();
                    return new[]
                    {
                        new Section
                        {
                            Id = expectedId,
                            Name = expectedName,
                            Products = Array.Empty<Product>(),
                        },
                    };
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetSections().Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(IEnumerable<SectionDTO>));
            var sections = (IEnumerable<SectionDTO>)viewResult.Value;
            Assert
                .AreEqual(expectedId, sections.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, sections.FirstOrDefault().Name);
            productDataMock
                .Verify(p => p.GetSections(), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetSectionById_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestSection";
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetSection(expectedId))
                .ReturnsAsync((int i) =>
                {
                    Task.Delay(1000).Wait();
                    return new Section
                    {
                        Id = expectedId,
                        Name = expectedName,
                        Products = Array.Empty<Product>(),
                    };
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetSectionById(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(SectionDTO));
            var section = (SectionDTO)viewResult.Value;
            Assert
                .AreEqual(expectedId, section.Id);
            Assert
                .AreEqual(expectedName, section.Name);
            productDataMock
                .Verify(p => p.GetSection(expectedId), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetBrands_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestBrand";
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetBrands())
                .ReturnsAsync(() =>
                {
                    Task.Delay(1000).Wait();
                    return new[]
                    {
                        new Brand
                        {
                            Id = expectedId,
                            Name = expectedName,
                            Products = Array.Empty<Product>(),
                        },
                    };
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetBrands().Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(IEnumerable<BrandDTO>));
            var brands = (IEnumerable<BrandDTO>)viewResult.Value;
            Assert
                .AreEqual(expectedId, brands.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, brands.FirstOrDefault().Name);
            productDataMock
                .Verify(p => p.GetBrands(), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetBrandById_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestBrand";
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetBrand(expectedId))
                .ReturnsAsync((int i) =>
                {
                    Task.Delay(1000).Wait();
                    return new Brand()
                    {
                        Id = expectedId,
                        Name = expectedName,
                        Products = Array.Empty<Product>(),
                    };
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetBrandById(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(BrandDTO));
            var brand = (BrandDTO)viewResult.Value;
            Assert
                .AreEqual(expectedId, brand.Id);
            Assert
                .AreEqual(expectedName, brand.Name);
            productDataMock
                .Verify(p => p.GetBrand(expectedId), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetProductsFilter_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestProduct";
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>(), true))
                .ReturnsAsync((ProductFilter f, bool b) =>
                {
                    Task.Delay(1000).Wait();
                    return new[]
                    {
                        new Product
                        {
                            Id = expectedId,
                            Name = expectedName,
                            OrderItems = Array.Empty<OrderItem>(),
                        },
                    };
                });
            var filter = new ProductFilter
            {
                Ids = new[] {1},
            };
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetProducts(filter).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(IEnumerable<ProductDTO>));
            var products = (IEnumerable<ProductDTO>)viewResult.Value;
            Assert
                .AreEqual(expectedId, products.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, products.FirstOrDefault().Name);
            productDataMock
                .Verify(p => p.GetProducts(filter, true), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetProduct_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestProduct";
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((int i) =>
                {
                    Task.Delay(1000).Wait();
                    return new Product
                    {
                        Id = expectedId,
                        Name = expectedName,
                        OrderItems = Array.Empty<OrderItem>(),
                    };
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetProduct(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(ProductDTO));
            var product = (ProductDTO)viewResult.Value;
            Assert
                .AreEqual(expectedId, product.Id);
            Assert
                .AreEqual(expectedName, product.Name);
            productDataMock
                .Verify(p => p.GetProductById(expectedId), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Add_Return_Correct()
        {
            const int expectedId = 1;
            var product = new ProductDTO
            {
                Id = expectedId,
                Name = "TestName",
                SectionId = 1,
                Section = new SectionDTO{Id = 1},
                BrandId = 1,
                Brand = new BrandDTO{Id = 1},
            };
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync((Product p) =>
                {
                    Task.Delay(1000).Wait();
                    return p.Id;
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.Add(product).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(int));
            Assert
                .AreEqual(expectedId, (int)viewResult.Value);
            productDataMock
                .Verify(p => p.AddProduct(It.IsAny<Product>()));
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Update_Act_Correct()
        {
            const int expectedId = 1;
            const string expectedNewName = "NewTestName";
            string callbackNewName = default;
            var product = new ProductDTO
            {
                Id = expectedId,
                Name = expectedNewName,
                SectionId = 1,
                Section = new SectionDTO{Id = 1},
                BrandId = 1,
                Brand = new BrandDTO{Id = 1},
            };
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.UpdateProduct(It.IsAny<Product>()))
                .Callback((Product p) => { callbackNewName = p.Name; });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.Update(product).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkResult));
            Assert
                .AreEqual(expectedNewName, callbackNewName);
            productDataMock
                .Verify(p => p.UpdateProduct(It.IsAny<Product>()));
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteOk_Return_Correct()
        {
            const int expectedId = 1;
            const bool expectedValue = true;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.DeleteProduct(It.IsAny<int>()))
                .ReturnsAsync(true);
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.Delete(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objectResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objectResult.Value, typeof(bool));
            Assert
                .AreEqual(expectedValue, (bool)objectResult.Value);
            productDataMock
                .Verify(p => p.DeleteProduct(expectedId));
            productDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteFailed_Return_CorrectNotFount()
        {
            const int expectedId = 1;
            const bool expectedValue = false;
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.DeleteProduct(It.IsAny<int>()))
                .ReturnsAsync(false);
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.Delete(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var objectResult = (NotFoundObjectResult) result;
            Assert
                .IsInstanceOfType(objectResult.Value, typeof(bool));
            Assert
                .AreEqual(expectedValue, (bool)objectResult.Value);
            productDataMock
                .Verify(p => p.DeleteProduct(expectedId));
            productDataMock
                .VerifyNoOtherCalls();
        }

        #endregion
    }
}
