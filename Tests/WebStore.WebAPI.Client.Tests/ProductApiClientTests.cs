using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Client.Tests
{
    [TestClass]
    public class ProductApiClientTests
    {
        [TestMethod]
        public void GetSections_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestSection";
            const int expectedCount = 3;
            var jsonResponse = JsonConvert.SerializeObject(
                Enumerable.Range(1, expectedCount).Select(
                    id => new Section()
                    {
                        Id = id,
                        Name = expectedName,
                        Products = Array.Empty<Product>(),
                    }));
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetSections().Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<Section>));
            Assert
                .AreEqual(expectedCount, actual.Count());
            Assert
                .AreEqual(expectedId, actual.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, actual.FirstOrDefault().Name);
        }

        [TestMethod]
        public void GetSection_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestSection";
            var jsonResponse = JsonConvert.SerializeObject(
                new Section()
                {
                    Id = expectedId,
                    Name = expectedName,
                    Products = Array.Empty<Product>(),
                });
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetSection(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Section));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
        }

    }
}
