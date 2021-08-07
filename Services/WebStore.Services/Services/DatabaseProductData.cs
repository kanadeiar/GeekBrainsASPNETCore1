using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.Models.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    /// <summary> Хранение данных в базе данных по товарам </summary>
    public class DatabaseProductData : IProductData
    {
        private readonly WebStoreContext _context;
        private readonly ILogger<DatabaseProductData> _logger;
        public DatabaseProductData(WebStoreContext context, ILogger<DatabaseProductData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Section>> GetSections() => await _context.Sections.Include(s => s.Products).ToArrayAsync().ConfigureAwait(false);
        public async Task<Section> GetSection(int id)
        {
            return await _context.Sections
                .Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
        }
        
        public async Task<IEnumerable<Brand>> GetBrands() => await _context.Brands.Include(s => s.Products).ToArrayAsync().ConfigureAwait(false);
        public async Task<Brand> GetBrand(int id)
        {
            return await _context.Brands
                .Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Tag>> GetTags() => await _context.Tags.ToArrayAsync().ConfigureAwait(false);

        public async Task<Tag> GetTag(int id)
        {
            return await _context.Tags
                .Include(t => t.Products).FirstOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
        }

        public async Task<ProductPage> GetProducts(IProductFilter productFilter = null, bool includes = false)
        {
            IQueryable<Product> query = includes
                ? _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Section)
                    //.Where(p => p.StateDelete != "1")
                : _context.Products;

            if (productFilter?.Ids?.Length > 0)
            {
                query = query.Where(p => productFilter.Ids.Contains(p.Id));
            }
            else
            {
                if (productFilter?.Name is { } name)
                    query = query.Where(q => q.Name.Contains(name));
                if (productFilter?.SectionId is { } sectionId)
                    query = query.Where(q => q.SectionId == sectionId);
                if (productFilter?.BrandId is { } brandId)
                    query = query.Where(q => q.BrandId == brandId);
            }

            var productsCount = await query.CountAsync().ConfigureAwait(false);

            _logger.LogInformation($"Запрос SQL: {query.ToQueryString()}");

            if (productFilter is {PageSize: > 0 and var pageSize, Page: > 0 and var page})
            {
                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
            }

            return new ProductPage()
            {
                Products = query.AsEnumerable(),
                TotalCount = productsCount,
            };
        }

        public async Task<Product> GetProductById(int id) => await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .Include(p => p.Tags)
                .Include(p => p.ImageUrls)
                .SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

        public async Task<int> AddProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));
            _context.Add(product);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            #region Лог
            _logger.LogInformation("Товар {0} {1} успешно добавлен в базу данных", product.Id, product.Name);
            #endregion
            return product.Id;
        }

        public async Task UpdateProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));
            if (_context.Products.Local.Any(e => e == product))
            {
                _context.Update(product);
                
            }
            else
            {
                var origin = await _context.Products
                    .Include(p => p.Tags)
                    .SingleOrDefaultAsync(p => p.Id == product.Id)
                    .ConfigureAwait(false);
                origin.Name = product.Name;
                origin.Order = product.Order;
                origin.SectionId = product.SectionId;
                origin.BrandId = product.BrandId;
                origin.Price = product.Price;
                origin.ImageUrl = product.ImageUrl;
                var ids = product.Tags.Select(t => t.Id);
                var tags = _context.Tags.Where(p => ids.Contains(p.Id));
                origin.Tags.Clear();
                foreach (var tag in tags) 
                    origin.Tags.Add(tag);
                _context.Update(origin);
            }
            await _context.SaveChangesAsync().ConfigureAwait(false);
            #region Лог
            _logger.LogInformation("Товар {0} {1} успешно обновлен в базе данных", product.Id, product.Name);
            #endregion
        }

        public async Task<bool> DeleteProduct(int id)
        {
            if (await GetProductById(id).ConfigureAwait(false) is { } product)
            //    //product.StateDelete = "1";
            //else
                return false;
            await _context.SaveChangesAsync();
            #region Лог
            //_logger.LogInformation("Товар {0} {1} успешно удален из базы данных", id, product.Name);
            #endregion
            return true;
        }
    }
}
