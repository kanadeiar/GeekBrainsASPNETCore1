using System;
using System.Security.Claims;
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
        #region Тестирование основных методов

        [TestMethod]
        public void Index_SendRequest_ShouldView()
        {
            var expectedCart = new CartWebModel();
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(_ => _.GetWebModel())
                .ReturnsAsync(expectedCart);
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceMock.Object, orderServiceStub, loggerStub);

            var result = controller.Index();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var model = (CartOrderWebModel)((ViewResult)result).Model;
            Assert
                .AreSame(expectedCart, model.Cart);
            cartServiceMock
                .Verify(_ => _.GetWebModel(), Times.Once);
            cartServiceMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Add_SendRequestId1_ShouldRedirect()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(_ => _.Add(It.IsAny<int>()));
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceMock.Object, orderServiceStub, loggerStub);

            var result = controller.Add(1);

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert
                .AreEqual("Cart", redirectResult.ControllerName);
            Assert
                .AreEqual(nameof(CartController.Index), redirectResult.ActionName);
            cartServiceMock
                .Verify(_ => _.Add(It.IsAny<int>()), Times.Once);
            cartServiceMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Subtract_SendRequestId1_ShouldRedirect()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(_ => _.Subtract(It.IsAny<int>()));
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceMock.Object, orderServiceStub, loggerStub);

            var result = controller.Subtract(1);

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert
                .AreEqual("Cart", redirectResult.ControllerName);
            Assert
                .AreEqual(nameof(CartController.Index), redirectResult.ActionName);
            cartServiceMock
                .Verify(_ => _.Subtract(It.IsAny<int>()), Times.Once);
            cartServiceMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Remove_SendRequestId1_ShouldRedirect()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(_ => _.Remove(It.IsAny<int>()));
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceMock.Object, orderServiceStub, loggerStub);

            var result = controller.Remove(1);

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert
                .AreEqual("Cart", redirectResult.ControllerName);
            Assert
                .AreEqual(nameof(CartController.Index), redirectResult.ActionName);
            cartServiceMock
                .Verify(_ => _.Remove(It.IsAny<int>()), Times.Once);
            cartServiceMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Clear_Do_ShouldRedirect()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(_ => _.Clear());
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceMock.Object, orderServiceStub, loggerStub);

            var result = controller.Clear();

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert
                .AreEqual("Cart", redirectResult.ControllerName);
            Assert
                .AreEqual(nameof(CartController.Index), redirectResult.ActionName);
            cartServiceMock
                .Verify(_ => _.Clear(), Times.Once);
            cartServiceMock
                .VerifyNoOtherCalls();
        }

        #endregion

        #region Тестирование метода оформления заказа

        [TestMethod]
        public async Task CheckOut_SendInvalidModelStateRequest_ShouldCorrectView()
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
        public async Task CheckOut_SendRequest_ShouldCreateAndRedirect()
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
                .Setup(_ => _.GetWebModel())
                .ReturnsAsync(new CartWebModel
                {
                    Items = new[]{ (new ProductWebModel
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
                .Setup(_ => _.CreateOrder(It.IsAny<string>(), It.IsAny<CartWebModel>(), It.IsAny<CreateOrderWebModel>()))
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

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert
                .AreEqual(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert
                .IsNull(redirectResult.ControllerName);
            Assert
                .AreEqual(expectedOrderId, redirectResult.RouteValues["Id"]);
            cartServiceMock
                .Verify(_ => _.GetWebModel());
            cartServiceMock
                .Verify(_ => _.Clear());
            cartServiceMock
                .VerifyNoOtherCalls();
            orderServiceMock
                .Verify(_ => _.CreateOrder(It.IsAny<string>(), It.IsAny<CartWebModel>(), It.IsAny<CreateOrderWebModel>()));
            orderServiceMock
                .VerifyNoOtherCalls();
        }

        #endregion

        #region Тестирование метода подтверждения заказа

        [TestMethod]
        public async Task OrderConfirmed_SendRequest_ShouldView()
        {
            const int expectedOrderId = 1;
            const string expectedName = "Test Name";
            var cartServiceStub = Mock
                .Of<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(_ => _.GetOrderById(It.IsAny<int>()))
                .ReturnsAsync(new Order { Name = expectedName });
            var loggerStub = Mock
                .Of<ILogger<CartController>>();
            var controller = new CartController(cartServiceStub, orderServiceMock.Object, loggerStub);

            var result = await controller.OrderConfirmed(expectedOrderId);

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var resultView = (ViewResult)result;
            Assert
                .AreEqual(expectedOrderId, resultView.ViewData["OrderId"]);
            Assert
                .AreEqual(expectedName, resultView.ViewData["Name"]);
        }

        #endregion
    }
}
