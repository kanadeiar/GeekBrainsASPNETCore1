using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Dal.Context;
using WebStore.Dal.Interfaces;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;

namespace WebStore.WebAPI.IntegrationTests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestMethod]
        public async Task Get_SendRequest_StandardHost_ShouldOk()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(DbContextOptions<WebStoreContext>));
                        services.Remove(descriptor);
                        services.AddDbContext<WebStoreContext>(options =>
                        {
                            options.UseInMemoryDatabase(nameof(Get_SendRequest_StandardHost_ShouldOk));
                        });
                        var descriptorInitializer =
                            services.SingleOrDefault(_ => _.ServiceType == typeof(IWebStoreDataInit));
                        services.Remove(descriptorInitializer);
                        services.AddTransient(_ => Mock.Of<IWebStoreDataInit>());
                    });
                });
            var testContext = webHost
                .Services.CreateScope().ServiceProvider.GetService<WebStoreContext>();
            testContext!.Products.AddRange(
                Enumerable.Range(1, 3).Select(i => new Product
                {
                    Name = "Товар",
                    Section = new Section (),
                    Brand = new Brand (),
                    OrderItems = Array.Empty<OrderItem>(),
                }));
            await testContext!.SaveChangesAsync();
            var httpClient = webHost.CreateClient();

            var response = await httpClient.GetAsync("Api/Value");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
