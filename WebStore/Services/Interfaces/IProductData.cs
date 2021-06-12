using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Domain.Infrastructure.Filters;
using WebStore.Domain.Infrastructure.Interfaces;

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        /// <summary> Все категории товаров </summary>
        public IEnumerable<Section> GetSections();
        /// <summary> Все бренды </summary>
        public IEnumerable<Brand> GetBrands();
        /// <summary> Все товары с фильтрацией по категориям и/или брендам </summary>
        public IEnumerable<Product> GetProducts(IProductFilter productFilter = null);

        IEnumerable<Section> GetSectionsWithProducts();
        IEnumerable<Brand> GetBrandsWithProducts();
    }
}
