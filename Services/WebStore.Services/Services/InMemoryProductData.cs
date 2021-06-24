using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.Models.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services
{
    /// <summary> Хранение товаров в оперативной памяти </summary>
    [Obsolete("Не использовать этот класс для хранения товаров", true)]
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
        public IEnumerable<Section> GetSectionsWithProducts() => GetSections();
        public IEnumerable<Brand> GetBrands() => _Brands;
        public IEnumerable<Brand> GetBrandsWithProducts() => GetBrands();
        public IEnumerable<Product> GetProducts(IProductFilter productFilter = null, bool includes = false)
        {
            IEnumerable<Product> query = _Products;
            if (productFilter?.SectionId is { } sectionId)
                query = query.Where(p => p.SectionId == sectionId);
            if (productFilter?.BrandId is { } brandId)
                query = query.Where(p => p.BrandId == brandId);
            return query;
        }
        public Product GetProductById(int id) => _Products.SingleOrDefault(p => p.Id == id);
        public int AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
