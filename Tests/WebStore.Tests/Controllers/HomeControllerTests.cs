using System;
using System.Collections.Generic;
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
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        #region Тест главного действия

        [TestMethod]
        public void Index_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock
                .Of<IConfiguration>();
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), false).Result)
                .Returns(new ProductPage{Products = Enumerable.Empty<Product>(), TotalCount = 0 });
            var controller = new HomeController(configurationStub);

            var result = controller.Index(productDataMock.Object);

            Assert
                .IsInstanceOfType(result.Result, typeof(ViewResult));
            var viewResult = (ViewResult) result.Result;
            Assert
                .IsInstanceOfType(viewResult.ViewData["Products"], typeof(IEnumerable<ProductWebModel>));
            Assert
                .IsInstanceOfType(viewResult.ViewData["CatagoryProducts"], typeof(IEnumerable<IEnumerable<ProductWebModel>>));
            Assert
                .IsInstanceOfType(viewResult.ViewData["RecommendedProducts"], typeof(IEnumerable<IEnumerable<ProductWebModel>>));
            productDataMock.Verify(_ => _.GetProducts(It.IsAny<ProductFilter>(), false), Times.Once);
            productDataMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Тесты второстепенных действий

        [TestMethod]
        public void ProductDetails_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.ProductDetails();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Checkout_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Checkout();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cart_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Cart();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Login_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Login();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Blog_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Blog();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void BlogSingle_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.BlogSingle();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ContactUs_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.ContactUs();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Error_SendRequest_ShouldCorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Error();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Second_SendRequest_ShouldCorrectString()
        {
            const string expectedString = "Test string";
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Second(expectedString);

            Assert
                .IsInstanceOfType(result, typeof(ContentResult));
            var contentResult = ((ContentResult) result).Content;
            Assert
                .AreEqual(expectedString, contentResult);
        }

        #endregion

        #region Тесты действия с выбрасыванием исключения

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void Throw_SendWithThrownRequest_ShouldApplicationException()
        {
            const string expectedString = "TestErrorMessage";
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            _ = controller.Throw(expectedString);
        }

        [TestMethod]
        public void ThrowTwo_SendWithThrownRequest_ShouldApplicationException()
        {
            const string expectedString = "TestErrorMessage";
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var exception = Assert.ThrowsException<ApplicationException>(() => controller.Throw(expectedString));

            Assert
                .AreEqual(expectedString, exception.Message);
        }

        #endregion
    }
}
