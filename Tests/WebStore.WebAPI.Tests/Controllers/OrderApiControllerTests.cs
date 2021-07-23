using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels.Cart;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Controllers;

namespace WebStore.WebAPI.Tests.Controllers
{
    [TestClass]
    public class OrderApiControllerTests
    {
        #region Тестирование апи контроллера заказов

        [TestMethod]
        public void GetUserOrders_Returns_Correct()
        {
            const string expectedName = "Test Name";
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(o => o.GetUserOrders(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                {
                    Task.Delay(1000).Wait();
                    return new[]
                    {
                        new Order
                        {
                            Id = 1,
                            Name = expectedName,
                            Items = Array.Empty<OrderItem>(),
                        },
                    };
                });
            var controller = new OrderApiController(orderServiceMock.Object);

            var result = controller.GetUserOrders(expectedName).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(IEnumerable<OrderDTO>));
            var values = ((IEnumerable<OrderDTO>) objResult.Value).ToArray();
            Assert
                .AreEqual(expectedName, values.FirstOrDefault()!.Name);
            orderServiceMock
                .Verify(o => o.GetUserOrders(It.IsAny<string>()), Times.Once);
            orderServiceMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetUserById_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "Test Name";
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(o => o.GetOrderById(It.IsAny<int>()))
                .ReturnsAsync((int i) =>
                {
                    Task.Delay(1000).Wait();
                    return new Order
                    {
                        Id = 1,
                        Name = expectedName,
                        Items = Array.Empty<OrderItem>(),
                    };
                });
            var controller = new OrderApiController(orderServiceMock.Object);

            var result = controller.GetOrderById(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(OrderDTO));
            var value = (OrderDTO) objResult.Value;
            Assert
                .AreEqual(expectedId, value.Id);
            orderServiceMock
                .Verify(o => o.GetOrderById(It.IsAny<int>()));
            orderServiceMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void CreateOrder_Returns_Correct()
        {
            const string expectedName = "Test name";
            const string expectedPhone = "999";
            const string expectedAddress = "Address";
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(o => o.CreateOrder(It.IsAny<string>(), It.IsAny<CartWebModel>(),
                    It.IsAny<CreateOrderWebModel>()))
                .ReturnsAsync((string name, CartWebModel cart, CreateOrderWebModel order) => {
                    Task.Delay(1000).Wait();
                    return new Order
                    {
                        Name = order.Name,
                        Phone = order.Phone,
                        Address = order.Address,
                        Items = Array.Empty<OrderItem>(),
                    };
                });
            var controller = new OrderApiController(orderServiceMock.Object);
            var dto = new CreateOrderDTO
            {
                Order = new CreateOrderWebModel
                {
                    Name = expectedName,
                    Address = expectedAddress,
                    Phone = expectedPhone,
                },
                Items = Array.Empty<OrderItemDTO>(),
            };

            var result = controller.CreateOrder(expectedName, dto).Result;
            
            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(OrderDTO));
            var value = (OrderDTO) objResult.Value;
            Assert
                .AreEqual(expectedName, value.Name);
            Assert
                .AreEqual(expectedPhone, value.Phone);
            Assert
                .AreEqual(expectedAddress, value.Address);
            orderServiceMock
                .Verify(o => o.CreateOrder(It.IsAny<string>(), It.IsAny<CartWebModel>(),
                    It.IsAny<CreateOrderWebModel>()), Times.Once);
            orderServiceMock
                .VerifyNoOtherCalls();
        }

        #endregion
    }
}
