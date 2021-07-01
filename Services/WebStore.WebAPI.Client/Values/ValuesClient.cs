using System.Collections.Generic;
using System.Linq;
using Polly;
using Polly.Retry;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Interfaces.WebAPI;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        private AsyncRetryPolicy _policy = Policy
            .Handle<HttpRequestException>()
            .RetryAsync(3);
        public ValuesClient(HttpClient client) : base(client, "api/values") { }

        public IEnumerable<string> GetAll()
        {
            var response = _policy.ExecuteAsync(async () => 
                await Client.GetAsync(Address)).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;
            return Enumerable.Empty<string>();
        }

        public string GetById(int id)
        {
            var response = _policy.ExecuteAsync(async () => 
                await Client.GetAsync($"{Address}/{id}")).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;
            return default;
        }

        public void Add(string str)
        {
            //var response = Client.PostAsJsonAsync(Address, str).Result;
            var response = Client.GetAsync($"{Address}/add?str={str}").Result;
            response.EnsureSuccessStatusCode();
        }

        public void Edit(int id, string str)
        {
            var response = Client.PutAsJsonAsync($"{Address}/{id}", str).Result;
            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int id)
        {
            var response = Client.DeleteAsync($"{Address}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
