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
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(s => s.GetProducts(It.IsAny<ProductFilter>(), false))
                .Returns(Enumerable.Empty<Product>());
            var controller = new HomeController(configurationStub);

            var result = controller.Index(productDataMock.Object);

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.ViewData["Products"], typeof(IEnumerable<ProductWebModel>));
        }

        [TestMethod]
        public void ProductDetails_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.ProductDetails();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Checkout_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Checkout();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cart_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Cart();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Login_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Login();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Blog_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Blog();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void BlogSingle_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.BlogSingle();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ContactUs_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.ContactUs();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Error_Returns_CorrectView()
        {
            var configurationStub = Mock.Of<IConfiguration>();
            var controller = new HomeController(configurationStub);

            var result = controller.Error();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Second_Returns_CorrectString()
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
    }
}
