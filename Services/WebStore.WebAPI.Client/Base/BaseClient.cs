using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Client.Base
{
    /// <summary> Базовая версия клиента доступа к веб апи </summary>
    public abstract class BaseClient
    {
        protected readonly HttpClient _client;
        protected readonly string _address;
        public BaseClient(HttpClient client, string address)
        {
            _client = client;
            _address = address;
        }
        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url).ConfigureAwait(false);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<T>();
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await _client.PostAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await _client.PutAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url).ConfigureAwait(false);
            return response;
        }
    }
}
