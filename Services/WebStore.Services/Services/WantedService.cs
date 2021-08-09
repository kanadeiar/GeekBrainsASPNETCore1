using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Product;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    public class WantedService : IWantedService
    {
        private readonly IWantedStore _wantedStore;
        private readonly IProductData _productData;
        private readonly Mapper _mapperProductToWeb = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));

        public WantedService(IWantedStore wantedStore, IProductData productData)
        {
            _wantedStore = wantedStore;
            _productData = productData;
        }

        public void Add(int id)
        {
            var wanted = _wantedStore.Wanted;

            if (!wanted.ProductsIds.Contains(id))
                wanted.ProductsIds.Add(id);

            _wantedStore.Wanted = wanted;
        }

        public void Remove(int id)
        {
            var wanted = _wantedStore.Wanted;

            if (wanted.ProductsIds.Contains(id))
                wanted.ProductsIds.Remove(id);

            _wantedStore.Wanted = wanted;
        }

        public void Clear()
        {
            var wanted = _wantedStore.Wanted;

            wanted.ProductsIds.Clear();

            _wantedStore.Wanted = wanted;
        }

        public async Task<WantedWebModel> GetWebModel()
        {
            var products = (await _productData.GetProducts(new ProductFilter
            {
                Ids = _wantedStore.Wanted.ProductsIds.ToArray(),
            }))?.Products;
            var productWebs = _mapperProductToWeb
                .Map<IEnumerable<ProductWebModel>>(products).ToDictionary(p => p.Id);

            return new WantedWebModel
            {
                Items = _wantedStore.Wanted.ProductsIds.Select(p => productWebs[p])
            };
        }
    }
}
