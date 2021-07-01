using System.Net.Http;

namespace WebStore.WebAPI.Client.Base
{
    /// <summary> Кастрированная синхронная базовая версия клиента веб апи </summary>
    public abstract class BaseSyncClient : BaseClient
    {
        protected BaseSyncClient(HttpClient client, string address) : base(client, address) { }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync<T>(url, item).Result;
        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync<T>(url, item).Result;
        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
    }
}
