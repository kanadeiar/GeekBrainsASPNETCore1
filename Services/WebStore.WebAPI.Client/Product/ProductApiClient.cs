using System.Net.Http;
using WebStore.Interfaces.Adresses;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Product
{
    /// <summary> Апи клиент товаров </summary>
    public class ProductApiClient : BaseSyncClient
    {
        public ProductApiClient(HttpClient client) : base(client, WebAPIInfo.Product) { }


    }
}
