using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Product;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    public class CompareService : ICompareService
    {
        private readonly ICompareStore _compareStore;
        private readonly IProductData _productData;
        private readonly Mapper _mapperProductToWeb = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));

        public bool IsMoreOne => _compareStore.Compare.ProductsIds.Count > 1;

        public CompareService(ICompareStore compareStore, IProductData productData)
        {
            _compareStore = compareStore;
            _productData = productData;
        }

        public void Add(int id)
        {
            var compare = _compareStore.Compare;

            if (!compare.ProductsIds.Contains(id))
                compare.ProductsIds.Add(id);

            _compareStore.Compare = compare;
        }

        public void Clear()
        {
            var compare = _compareStore.Compare;

            compare.ProductsIds.Clear();

            _compareStore.Compare = compare;
        }

        public async Task<CompareWebModel> GetWebModel()
        {
            var products = (await _productData.GetProducts(new ProductFilter
            {
                Ids = _compareStore.Compare.ProductsIds.ToArray(),
            }))?.Products;
            var productWebs = _mapperProductToWeb
                .Map<IEnumerable<ProductWebModel>>(products).ToDictionary(p => p.Id);

            return new CompareWebModel
            {
                Items = _compareStore.Compare.ProductsIds.Select(p => productWebs[p])
            };
        }
    }
}
