using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Mappers;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.Models.Interfaces;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Product
{
    /// <summary> Апи клиент товаров </summary>
    public class ProductApiClient : BaseSyncClient, IProductData
    {
        public ProductApiClient(HttpClient client) : base(client, WebAPIInfo.ApiProduct) { }


        public IEnumerable<Section> GetSections()
        {
            return Get<IEnumerable<SectionDTO>>($"{Address}/section").FromDTO();
        }

        public Section GetSection(int id)
        {
            return Get<SectionDTO>($"{Address}/section/{id}").FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return Get<IEnumerable<BrandDTO>>($"{Address}/brand").FromDTO();
        }

        public Brand GetBrand(int id)
        {
            return Get<BrandDTO>($"{Address}/brand/{id}").FromDTO();
        }

        public async Task<IEnumerable<Domain.Entities.Product>> GetProducts(IProductFilter productFilter = null, bool includes = false)
        {
            var response = await PostAsync(Address, productFilter ?? new ProductFilter());
            var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
            return products.FromDTO();
        }

        public Domain.Entities.Product GetProductById(int id)
        {
            return Get<ProductDTO>($"{Address}/{id}").FromDTO();
        }

        public int AddProduct(Domain.Entities.Product product)
        {
            var response = Post($"{Address}/product", product.ToDTO());
            var id = response.Content.ReadFromJsonAsync<int>().Result;
            return id;
        }

        public void UpdateProduct(Domain.Entities.Product product)
        {
            Put($"{Address}/product", product.ToDTO());
        }

        public bool DeleteProduct(int id)
        {
            var result = Delete($"{Address}/product/{id}").IsSuccessStatusCode;
            return result;
        }
    }
}
