﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.WebAPI.Client.Tests
{
    [TestClass]
    public class WorkerApiClientTests
    {
        [TestMethod]
        public void GetAll_3Worker_ShouldEnumerable()
        {
            const int expectedId = 1;
            const string expectedFam = "Ivanov";
            const string expectedName = "Ivan";
            const int expectedCount = 3;
            var jsonResponse = JsonConvert.SerializeObject(
                Enumerable.Range(expectedId, expectedCount).Select(
                    id => new Worker()
                    {
                        Id = id,
                        LastName = expectedFam,
                        FirstName = expectedName,                        
                    }));
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new WorkerApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.GetAll().Result;

            Assert
                .IsInstanceOfType(actual, typeof(IEnumerable<Worker>));
            Assert
                .AreEqual(expectedCount, actual.Count());
            Assert
                .AreEqual(expectedId, actual.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedFam, actual.FirstOrDefault().LastName);
            Assert
                .AreEqual(expectedName, actual.FirstOrDefault().FirstName);
            mockMessageHandler.Protected()
                .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Get_1Worker_ShouldOne()
        {
            const int expectedId = 1;
            const string expectedFam = "Ivanov";
            const string expectedName = "Ivan";
            var jsonResponse = JsonConvert.SerializeObject(
                new Worker()
                {
                    Id = expectedId,
                    LastName = expectedFam,
                    FirstName = expectedName,
                });
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });
            var client = new WorkerApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.Get(expectedId).Result;

            Assert
                .IsInstanceOfType(actual, typeof(Worker));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedFam, actual.LastName);
            Assert
                .AreEqual(expectedName, actual.FirstName);
            mockMessageHandler.Protected()
                .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Add_CorrectWorker_ShouldCorrectId()
        {
            const int expectedId = 1;
            const string fam = "Ivanov";
            const string name = "Ivan";
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
            var worker = new Worker
            {
                Id = expectedId,
                LastName = fam,
                FirstName = name,
            };
            var client = new WorkerApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.Add(worker).Result;

            Assert
                .IsInstanceOfType(actual, typeof(int));
            Assert
                .AreEqual(expectedId, actual);
            mockMessageHandler.Protected()
                .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Update_CorrectWorker_ShouldCorrectNewName()
        {
            const int expectedId = 1;
            const string expectedNewFam = "Ivanov";
            const string expectedNewName = "Ivan";
            string callbackNewFam = default;
            string callbackNewName = default;
            var worker = new Worker
            {
                Id = expectedId,
                LastName = expectedNewFam,
                FirstName = expectedNewName,
            };
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage m, CancellationToken c) => {
                    Task.Delay(1000).Wait();
                    var resultNormal = m.Content.ReadFromJsonAsync<Worker>().Result;
                    callbackNewFam = resultNormal.LastName;
                    callbackNewName = resultNormal.FirstName;
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                    };
                });
            var client = new WorkerApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            client.Update(worker).Wait();

            Assert
                .AreEqual(expectedNewFam, callbackNewFam);
            Assert
                .AreEqual(expectedNewName, callbackNewName);
            mockMessageHandler.Protected()
                .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteOk_CorrectId_ShouldTrue()
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
            var client = new WorkerApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.Delete(expectedId).Result;

            Assert
                .AreEqual(expectedValue, actual);
            mockMessageHandler.Protected()
                .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteProductNotFound_NotCorrectId_ShouldFalse()
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
            var client = new WorkerApiClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

            var actual = client.Delete(expectedId).Result;

            Assert
                .AreEqual(expectedValue, actual);
            mockMessageHandler.Protected()
                .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            mockMessageHandler
                .VerifyNoOtherCalls();
        }

    }
}
