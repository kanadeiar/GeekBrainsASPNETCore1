using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Client.Base
{
    /// <summary> Базовая версия клиента доступа к веб апи </summary>
    public abstract class BaseClient
    {
        protected readonly HttpClient Client;
        protected readonly string Address;
        public BaseClient(HttpClient client, string address)
        {
            Client = client;
            Address = address;
        }
        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await Client.GetAsync(url).ConfigureAwait(false);
            return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<T>();
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await Client.PostAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await Client.PutAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await Client.DeleteAsync(url).ConfigureAwait(false);
            return response;
        }
    }
}
