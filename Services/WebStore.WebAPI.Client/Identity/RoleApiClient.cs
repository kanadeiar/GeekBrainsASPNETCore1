using System.Net.Http;
using WebStore.Interfaces.Adresses;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Identity
{
    public class RoleApiClient : BaseClient
    {
        public RoleApiClient(HttpClient client) : base(client, WebAPIInfo.Identity.ApiRole) { }



    }
}
