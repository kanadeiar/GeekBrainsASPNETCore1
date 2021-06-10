using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.Dal.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Infrastructure.Filters;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DatabaseProductData : IProductData
    {
        private readonly WebStoreContext _context;
        public DatabaseProductData(WebStoreContext context)
        {
            _context = context;
        }
        public IEnumerable<Section> GetSections() => _context.Sections;
        public IEnumerable<Section> GetSectionsWithProducts() => _context.Sections.Include(s => s.Products);
        public IEnumerable<Brand> GetBrands() => _context.Brands;
        public IEnumerable<Brand> GetBrandsWithProducts() => _context.Brands.Include(b => b.Products);
        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            IQueryable<Product> query = _context.Products;
            if (productFilter?.SectionId is { } sectionId)
                query = query.Where(q => q.SectionId == sectionId);
            if (productFilter?.BrandId is { } brandId)
                query = query.Where(q => q.BrandId == brandId);
            return query;
        }
    }
}
