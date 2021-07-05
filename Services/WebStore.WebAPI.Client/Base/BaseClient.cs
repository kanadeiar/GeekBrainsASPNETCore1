using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Client.Base
{
    /// <summary> Базовая версия клиента доступа к веб апи </summary>
    public abstract class BaseClient : IDisposable
    {
        /// <summary> Клиент отправки запроса к веб апи серверу </summary>
        protected readonly HttpClient Client;
        /// <summary> адрес веб апи сервера </summary>
        protected readonly string Address;
        public BaseClient(HttpClient client, string address)
        {
            Client = client;
            Address = address;
        }

        #region Уничтожение объекта

        public void Dispose() => Dispose(true);
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing)
            {
                //уничтожить управляемые ресурсы
            }
            //уничтожить неуправляемые ресурсы
        }

        #endregion
        
        /// <summary> Асинхронное получение данных с веб апи сервера </summary>
        /// <typeparam name="T">тип данных</typeparam>
        /// <param name="url">конечная точка</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>данные</returns>
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await Client
                .GetAsync(url, cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content.ReadFromJsonAsync<T>(cancellationToken: cancel);
        }
        /// <summary> Асинхронное добавление данных в веб апи сервер </summary>
        /// <typeparam name="T">тип данных</typeparam>
        /// <param name="url">конечная точка</param>
        /// <param name="item">данные</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>статус добавления</returns>
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await Client
                .PostAsJsonAsync(url, item, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        /// <summary> Асинхронное обновление данных в веб апи сервере </summary>
        /// <typeparam name="T">тип данных</typeparam>
        /// <param name="url">конечная точка</param>
        /// <param name="item">данные</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>результат обновления</returns>
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await Client
                .PutAsJsonAsync(url, item, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        /// <summary> Асинхронное удаление данных из веб апи сервера </summary>
        /// <param name="url">конечная точка</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>результат обновления</returns>
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
        {
            var response = await Client
                .DeleteAsync(url, cancel).ConfigureAwait(false);
            return response;
        }
    }
}
