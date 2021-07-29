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

namespace WebStore.WebAPI.Client
{
    /// <summary> Апи клиент товаров </summary>
    public class ProductApiClient : BaseClient, IProductData
    {
        public ProductApiClient(HttpClient client) : base(client, WebAPIInfo.ApiProduct) { }


        public async Task<IEnumerable<Section>> GetSections()
        {
            return (await GetAsync<IEnumerable<SectionDTO>>($"{Address}/section").ConfigureAwait(false)).FromDTO();
        }

        public async Task<Section> GetSection(int id)
        {
            return (await GetAsync<SectionDTO>($"{Address}/section/{id}").ConfigureAwait(false)).FromDTO();
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            return (await GetAsync<IEnumerable<BrandDTO>>($"{Address}/brand").ConfigureAwait(false)).FromDTO();
        }

        public async Task<Brand> GetBrand(int id)
        {
            return (await GetAsync<BrandDTO>($"{Address}/brand/{id}").ConfigureAwait(false)).FromDTO();
        }

        public async Task<ProductPage> GetProducts(IProductFilter productFilter = null, bool includes = false)
        {
            var response = await PostAsync(Address, productFilter ?? new ProductFilter()).ConfigureAwait(false);
            var productPage = await response.Content.ReadFromJsonAsync<ProductPageDTO>();
            return productPage.FromDTO();
        }

        public async Task<Product> GetProductById(int id)
        {
            return (await GetAsync<ProductDTO>($"{Address}/{id}")).FromDTO();
        }

        public async Task<int> AddProduct(Product product)
        {
            var response = await PostAsync($"{Address}/product", product.ToDTO()).ConfigureAwait(false);
            var id = await response.Content.ReadFromJsonAsync<int>();
            return id;
        }

        public async Task UpdateProduct(Product product)
        {
            var _ = await PutAsync($"{Address}/product", product.ToDTO()).ConfigureAwait(false);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var result = (await DeleteAsync($"{Address}/product/{id}").ConfigureAwait(false)).IsSuccessStatusCode;
            return result;
        }
    }
}
