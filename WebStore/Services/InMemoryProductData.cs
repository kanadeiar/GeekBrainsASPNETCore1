using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Domain.Infrastructure.Filters;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryProductData : IProductData
    {
        private readonly List<Section> _Sections;
        private readonly List<Brand> _Brands;

        public InMemoryProductData(TestData testData)
        {
            _Sections = testData.GetSections.ToList();
            _Brands = testData.GetBrands.ToList();
        }

        public IEnumerable<Section> GetSections() => _Sections;

        public IEnumerable<Brand> GetBrands() => _Brands;

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            return null;
        }
    }
}
