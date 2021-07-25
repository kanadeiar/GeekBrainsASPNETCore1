﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.IntegrationTests.Controllers
{
    [TestClass]
    public class ProductApiControllerTests
    {
        [TestMethod]
        public async Task GetProducts_SendRequest_ReplaceService_ShouldOk()
        {
            Mock<IProductData> productDataMock = null;
            var webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(IWorkerData));
                        services.Remove(descriptor);
                        productDataMock = new Mock<IProductData>();
                        productDataMock
                            .Setup(_ => _.GetProducts(It.IsAny<ProductFilter>(), It.IsAny<bool>()))
                            .ReturnsAsync((ProductFilter filter, bool _) =>
                            {
                                return Enumerable.Range(1, 3)
                                    .Select(i => new Product
                                    {
                                        Id = i, 
                                        Name = "Товар",
                                        SectionId = 1,
                                        Section = new Section { Id = 1 },
                                        BrandId = 1,
                                        Brand = new Brand { Id = 1 },
                                        OrderItems = Array.Empty<OrderItem>(),
                                    });
                            });
                        services.AddTransient(_ => productDataMock.Object);
                    });
                });
            var filter = new ProductFilter();
            var httpClient = webHost.CreateClient();

            var response = await httpClient.PostAsJsonAsync("Api/Product", filter);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            productDataMock.Verify(_ => _.GetProducts(It.IsAny<ProductFilter>(), It.IsAny<bool>()));
            productDataMock.Verify();
        }


    }
}
