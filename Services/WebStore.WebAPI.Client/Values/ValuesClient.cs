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
                await _client.GetAsync(_address)).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;
            return Enumerable.Empty<string>();
        }

        public string GetById(int id)
        {
            var response = _policy.ExecuteAsync(async () => 
                await _client.GetAsync($"{_address}/{id}")).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;
            return default;
        }

        public void Add(string str)
        {
            //var response = _client.PostAsJsonAsync(_address, str).Result;
            var response = _client.GetAsync($"{_address}/add?str={str}").Result;
            response.EnsureSuccessStatusCode();
        }

        public void Edit(int id, string str)
        {
            var response = _client.PutAsJsonAsync($"{_address}/{id}", str).Result;
            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int id)
        {
            var response = _client.DeleteAsync($"{_address}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
