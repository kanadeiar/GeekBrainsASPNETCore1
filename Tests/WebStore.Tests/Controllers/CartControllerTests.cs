using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Cart;
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        #region Тестирование метода оформления заказа

        [TestMethod]
        public async Task CheckOut_ModelStateInvalid_Returns_CorrectView()
        {
            const string expectedOrderName = "Test name order";
            var cartServiceStub = Mock
                .Of<ICartService>();
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceStub, orderServiceStub, loggerStub);
            controller.ModelState.AddModelError("error", "InvalidError");
            var orderModel = new CreateOrderWebModel
            {
                Name = expectedOrderName,
            };

            var result = await controller.CheckOut(orderModel);

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(CartOrderWebModel));
            var cartModel = (CartOrderWebModel) viewResult.Model;
            Assert
                .AreEqual(expectedOrderName, cartModel.Order.Name);
        }

        [TestMethod]
        public async Task CheckOut_ModelStateValid_CallService_And_Redirect()
        {
            const int expectedProductId = 1;
            const string expectedProductName = "Test product";
            const decimal expectedProductPrice = 1m;
            const int expectedProductCount = 1;
            const int expectedOrderId = 1;
            const string expectedOrderName = "Test order";
            const string expectedOrderAddress = "Test address";
            const string expectedOrderPhone = "123";
            const string expectedUserName = "TestUser";
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(c => c.GetWebModel())
                .Returns(new CartWebModel
                {
                    Items = new []{ (new ProductWebModel
                    {
                        Id = expectedProductId,
                        Name = expectedProductName,
                        Price = expectedProductPrice,
                        Brand = "Test brand",
                        Section = "Test section",
                        ImageUrl = "TestImage.jpg",
                    }, expectedProductCount, expectedProductPrice * expectedProductCount) }
                });
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(o => o.CreateOrder(It.IsAny<string>(), It.IsAny<CartWebModel>(), It.IsAny<CreateOrderWebModel>()))
                .ReturnsAsync(new Order
                {
                    Id = expectedOrderId,
                    Name = expectedOrderName,
                    Address = expectedOrderAddress,
                    Phone = expectedOrderPhone,
                    DateTime = DateTime.Now,
                    Items = Array.Empty<OrderItem>(),
                });
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceMock.Object, orderServiceMock.Object, loggerStub)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new [] {new Claim(ClaimTypes.Name, expectedUserName)}))
                    }
                }
            };
            var orderModel = new CreateOrderWebModel
            {
                Name = expectedOrderName,
                Address = expectedOrderAddress,
                Phone = expectedOrderPhone,
            };

            var result = await controller.CheckOut(orderModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert.AreEqual(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual(expectedOrderId, redirectResult.RouteValues["Id"]);
        }

        #endregion
    }
}
