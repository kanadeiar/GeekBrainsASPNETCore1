using System.Net.Http;

namespace WebStore.WebAPI.Client.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _client;
        protected readonly string _address;
        public BaseClient(HttpClient client, string address)
        {
            _client = client;
            _address = address;
        }
    }
}
