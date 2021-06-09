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
        private readonly List<Product> _Products;

        public InMemoryProductData(TestData testData)
        {
            _Sections = testData.GetSections.ToList();
            _Brands = testData.GetBrands.ToList();
            _Products = testData.GetProducts.ToList();
        }

        public IEnumerable<Section> GetSections() => _Sections;

        public IEnumerable<Brand> GetBrands() => _Brands;

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            IEnumerable<Product> query = _Products;
            if (productFilter?.SectionId is { } sectionId)
                query = query.Where(p => p.SectionId == sectionId);
            if (productFilter?.BrandId is { } brandId)
                query = query.Where(p => p.BrandId == brandId);
            return query;
        }
    }
}
