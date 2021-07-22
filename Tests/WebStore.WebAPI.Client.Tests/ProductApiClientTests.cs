using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

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
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
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
                })
                .Verifiable();
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetSection(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Section));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetBrands_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestBrand";
            const int expectedCount = 3;
            var jsonResponse = JsonConvert.SerializeObject(
                Enumerable.Range(1, expectedCount).Select(
                    id => new Brand()
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

            var actual = client.GetBrands().Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<Brand>));
            Assert
                .AreEqual(expectedCount, actual.Count());
            Assert
                .AreEqual(expectedId, actual.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, actual.FirstOrDefault().Name);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetBrand_Returns_Corrent()
        {
            const int expectedId = 1;
            const string expectedName = "TestBrand";
            var jsonResponse = JsonConvert.SerializeObject(
                new Brand()
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

            var actual = client.GetBrand(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Brand));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetProductsFilter_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestProduct";
            const int expectedCount = 3;
            var jsonResponse = JsonConvert.SerializeObject(
                Enumerable.Range(1, expectedCount).Select(
                    id => new ProductDTO()
                    {
                        Id = id,
                        Name = expectedName,
                        SectionId = 1,
                        Section = new SectionDTO { Id = 1 },
                        BrandId = 1,
                        Brand = new BrandDTO { Id = 1 },
                    }));
            var filter = new ProductFilter
            {
                Ids = new[] { 1, 2, 3 },
            };
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetProducts(filter).Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<Product>));
            Assert
                .AreEqual(expectedCount, actual.Count());
            Assert
                .AreEqual(expectedId, actual.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, actual.FirstOrDefault().Name);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetProductById_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestProduct";
            var jsonResponse = JsonConvert.SerializeObject(
                new ProductDTO()
                {
                    Id = expectedId,
                    Name = expectedName,
                    SectionId = 1,
                    Section = new SectionDTO { Id = 1 },
                    BrandId = 1,
                    Brand = new BrandDTO { Id = 1 },
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

            var actual = client.GetProductById(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Product));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void AddProduct_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestProduct";
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    Task.Delay(1000).Wait();
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(expectedId.ToString()),
                    };
                });
            var product = new Product
            {
                Id = expectedId,
                Name = expectedName,
                SectionId = 1,
                Section = new Section { Id = 1 },
                BrandId = 1,
                Brand = new Brand { Id = 1 },
            };
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.AddProduct(product).Result;

            Assert
                .IsInstanceOfType(actual, typeof(int));
            Assert
                .AreEqual(expectedId, actual);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void UpdateProduct_Act_Correct()
        {
            const int expectedId = 1;
            const string expectedNewName = "TestProduct";
            string callbackNewName = default;
            var product = new Product
            {
                Id = expectedId,
                Name = expectedNewName,
                SectionId = 1,
                Section = new Section { Id = 1 },
                BrandId = 1,
                Brand = new Brand { Id = 1 },
            };
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage m, CancellationToken c) => {
                    Task.Delay(1000).Wait();
                    callbackNewName = (m.Content.ReadFromJsonAsync<ProductDTO>()).Result.Name;
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                });
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            client.UpdateProduct(product).Wait();

            Assert
                .AreEqual(expectedNewName, callbackNewName);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteProductOk_Returns_Correct()
        {
            const int expectedId = 1;
            const bool expectedValue = true;
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage m, CancellationToken c) => {
                    Task.Delay(1000).Wait();
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(expectedValue.ToString()),
                    };
                });
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.DeleteProduct(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(bool));
            Assert
                .AreEqual(expectedValue, actual);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteProductNotFound_Returns_Correct()
        {
            const int expectedId = 1;
            const bool expectedValue = false;
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage m, CancellationToken c) => {
                    Task.Delay(1000).Wait();
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Content = new StringContent(expectedValue.ToString()),
                    };
                });
            var client = new ProductApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.DeleteProduct(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(bool));
            Assert
                .AreEqual(expectedValue, actual);
            mockMessageHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler.VerifyNoOtherCalls();
        }
    }
}
