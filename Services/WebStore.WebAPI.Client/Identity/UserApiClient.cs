using System.Net.Http;
using WebStore.Interfaces.Adresses;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Identity
{
    public class UserApiClient : BaseClient
    {
        public UserApiClient(HttpClient client) : base(client, WebAPIInfo.Identity.ApiUser) { }



    }
}
