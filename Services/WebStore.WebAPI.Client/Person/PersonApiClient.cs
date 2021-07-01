using System.Net.Http;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Person
{
    public class PersonApiClient : BaseSyncClient
    {
        public PersonApiClient(HttpClient client, string address) : base(client, address) { }


    }
}
