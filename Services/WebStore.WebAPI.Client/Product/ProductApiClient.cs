using System.Collections.Generic;
using System.Net.Http;
using WebStore.Domain.Entities;
using WebStore.Domain.Models.Interfaces;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Product
{
    /// <summary> Апи клиент товаров </summary>
    public class ProductApiClient : BaseSyncClient, IProductData
    {
        public ProductApiClient(HttpClient client) : base(client, WebAPIInfo.Product) { }


        public IEnumerable<Section> GetSections()
        {
            throw new System.NotImplementedException();
        }

        public Section GetSection(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Brand> GetBrands()
        {
            throw new System.NotImplementedException();
        }

        public Brand GetBrand(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Domain.Entities.Product> GetProducts(IProductFilter productFilter = null, bool includes = false)
        {
            throw new System.NotImplementedException();
        }

        public Domain.Entities.Product GetProductById(int id)
        {
            throw new System.NotImplementedException();
        }

        public int AddProduct(Domain.Entities.Product product)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateProduct(Domain.Entities.Product product)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
