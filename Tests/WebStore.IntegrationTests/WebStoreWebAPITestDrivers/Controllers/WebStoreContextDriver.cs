using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using WebStore.Dal.Context;
using WebStore.Dal.Interfaces;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Models;

namespace WebStore.IntegrationTests.WebStoreWebAPITestDrivers.Controllers
{
    /// <summary> Драйвер для веб апи сервиса </summary>
    public class WebStoreContextDriver
    {
        /// <summary> Получение всех товаров </summary>
        public async Task<HttpResponseMessage> GetProducts(ProductFilter filter = null, int count = 3)
        {
            var httpClient = await GetTestHttpClientDriver();

            var response = await httpClient.PostAsJsonAsync("Api/Product", filter);

            return response;
        }
        public async Task<HttpResponseMessage> GetSections()
        {
            var httpClient = await GetTestHttpClientDriver();

            var response = await httpClient.GetAsync("Api/Product/Section");

            return response;
        }

        public async Task<HttpResponseMessage> GetBrands()
        {
            var httpClient = await GetTestHttpClientDriver();

            var response = await httpClient.GetAsync("Api/Product/Brand");

            return response;
        }

        private static async Task<HttpClient> GetTestHttpClientDriver()
        {
            var webHost = new WebApplicationFactory<WebAPI.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(DbContextOptions<WebStoreContext>));
                        services.Remove(descriptor);
                        services.AddDbContext<WebStoreContext>(options =>
                        {
                            options.UseInMemoryDatabase(nameof(WebStoreContext));
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
                    Name = $"Товар_{i}",
                    Section = new Section(),
                    Brand = new Brand(),
                    OrderItems = Array.Empty<OrderItem>(),
                }));
            testContext!.Sections.AddRange(
                Enumerable.Range(1, 3).Select(i => new Section
                {
                    Name = $"Секция_{i}",
                }));
            testContext!.Brands.AddRange(
                Enumerable.Range(1, 3).Select(i => new Brand
                {
                    Name = $"Бренд_{i}",
                }));
            await testContext!.SaveChangesAsync();
            var httpClient = webHost.CreateClient();
            return httpClient;
        }


    }
}
