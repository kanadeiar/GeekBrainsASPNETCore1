using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebStore.WebAPI.IntegrationTests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestMethod]
        public async Task Get_SendRequest_StandardHost_ShouldOk()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => {});
            HttpClient httpClient = webHost.CreateClient();

            var response = await httpClient.GetAsync("Api/Value");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
