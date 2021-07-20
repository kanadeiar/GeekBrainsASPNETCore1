using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client
{
    /// <summary> Апи клиент сотрудников </summary>
    public class WorkerApiClient : BaseClient, IWorkerData
    {
        public WorkerApiClient(HttpClient client) : base(client, WebAPIInfo.ApiWorker) { }

        public async Task<IEnumerable<Worker>> GetAll()
        {
            return await GetAsync<IEnumerable<Worker>>(Address).ConfigureAwait(false);
        }

        public async Task<Worker> Get(int id)
        {
            return await GetAsync<Worker>($"{Address}/{id}").ConfigureAwait(false);
        }

        public async Task<int> Add(Worker worker)
        {
            var response = await PostAsync(Address, worker).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task Update(Worker worker)
        {
            await PutAsync(Address, worker);
        }

        public async Task<bool> Delete(int id)
        {
            var result = (await DeleteAsync($"{Address}/{id}")).IsSuccessStatusCode;
            return result;
        }
    }
}
