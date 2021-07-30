using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.WebAPI.Client.Tests
{
    [TestClass]
    public class OrderApiClientTests
    {
        [TestMethod]
        public void GetUserOrders_3Order_ShouldEnumerable()
        {
            const int expectedId = 1;
            const string expectedName = "Test";
            const int expectedCount = 3;
            var jsonResponse = JsonConvert.SerializeObject(new[]
                {
                    new Order
                    {
                        Id = expectedId,
                        Name = expectedName,
                        Items = Array.Empty<OrderItem>(),
                    },
                    new Order { Items = Array.Empty<OrderItem>() },
                    new Order { Items = Array.Empty<OrderItem>() },
                });
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new OrderApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetUserOrders(expectedName).Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<Order>));
            Assert
                .AreEqual(expectedCount, actual.Count());
            Assert
                .AreEqual(expectedId, actual.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, actual.FirstOrDefault().Name);
            mockMessageHandler
                .Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetOrderById_1Order_ShouldOne()
        {
            const int expectedId = 1;
            const string expectedName = "Test";
            var jsonResponse = JsonConvert.SerializeObject(
                new Order
                {
                    Id = expectedId,
                    Name = expectedName,
                    Items = Array.Empty<OrderItem>(),
                });
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new OrderApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetOrderById(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Order));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            mockMessageHandler
                .Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void CreateOrder_CreateAdmin_ShouldOne()
        {
            const int expectedId = 1;
            const string expectedName = "Test";
            var jsonResponse = JsonConvert.SerializeObject(
            new Order
            {
                Id = expectedId,
                Name = expectedName,
                Items = Array.Empty<OrderItem>(),
            });
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new OrderApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });
            var cart = new CartWebModel
            {
                Items = Enumerable.Range(1, 1).Select(i => ( new ProductWebModel { Id = i, Name = $"TestName{i}" }, i )),
            };
            var order = new CreateOrderWebModel()
            {
                Name = "Test",
                Address = "Test",
                Phone = "123",
            };

            var actual = client.CreateOrder("Admin", cart, order).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Order));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            mockMessageHandler
                .Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

    }
}
