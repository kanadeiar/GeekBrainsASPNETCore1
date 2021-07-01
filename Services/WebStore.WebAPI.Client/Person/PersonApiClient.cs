using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Person
{
    public class PersonApiClient : BaseSyncClient, IWorkerData
    {
        public PersonApiClient(HttpClient client, string address) : base(client, address) { }

        public IEnumerable<Worker> GetAll()
        {
            return Get<IEnumerable<Worker>>(_address);
        }

        public Worker Get(int id)
        {
            return Get<Worker>($"{_address}/{id}");
        }

        public int Add(Worker worker)
        {
            var response = Post(_address, worker);
            return response.Content.ReadFromJsonAsync<int>().Result;
        }

        public void Update(Worker worker)
        {
            Put(_address, worker);
        }

        public bool Delete(int id)
        {
            var result = Delete($"{_address}/{id}").IsSuccessStatusCode;
            return result;
        }
    }
}
