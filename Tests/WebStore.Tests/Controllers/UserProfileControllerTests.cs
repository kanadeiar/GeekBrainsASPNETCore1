using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebStore.Controllers;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels.UserProfile;
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class UserProfileControllerTests
    {
        #region Тестирование отображения данных пользователя

        [TestMethod]
        public void Index_SendRequest_ShouldCorrectView()
        {
            var controller = new UserProfileController();

            var result = controller.Index();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Orders_SendRequest_ShouldCorrectView()
        {
            const int expectedId = 1;
            const string expectedName = "Test Order";
            const int expectedCount = 3;
            const string userName = "TestUser";
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(_ => _.GetUserOrders(userName))
                .ReturnsAsync((string _) =>
                    Enumerable.Range(1, expectedCount).Select(id => new Order
                    {
                        Id = id,
                        Name = expectedName,
                        Items = Array.Empty<OrderItem>()
                    }));
            var controller = new UserProfileController()
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }))
                    }
                }
            };

            var result = controller.Orders(orderServiceMock.Object).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(IEnumerable<UserOrderWebModel>));
            var orders = (IEnumerable<UserOrderWebModel>)viewResult.Model;
            Assert
                .AreEqual(expectedCount, orders.Count());
            Assert
                .AreEqual(expectedId, orders.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, orders.FirstOrDefault().Name);
            orderServiceMock
                .Verify(_ => _.GetUserOrders(userName), Times.Once);
            orderServiceMock
                .VerifyNoOtherCalls();
        }

        #endregion
    }
}
