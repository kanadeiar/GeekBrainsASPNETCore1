using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Person
{
    /// <summary> Апи клиент сотрудников </summary>
    public class WorkerApiClient : BaseSyncClient, IWorkerData
    {
        public WorkerApiClient(HttpClient client) : base(client, WebAPIInfo.Worker) { }

        public IEnumerable<Worker> GetAll()
        {
            return Get<IEnumerable<Worker>>(Address);
        }

        public Worker Get(int id)
        {
            return Get<Worker>($"{Address}/{id}");
        }

        public int Add(Worker worker)
        {
            var response = Post(Address, worker);
            return response.Content.ReadFromJsonAsync<int>().Result;
        }

        public void Update(Worker worker)
        {
            Put(Address, worker);
        }

        public bool Delete(int id)
        {
            var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
            return result;
        }
    }
}
