﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.Entities;
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

        public IEnumerable<Section> GetSections() => _context.Sections.Include(s => s.Products);
        public Section GetSection(int id)
        {
            return _context.Sections
                .Include(s => s.Products).FirstOrDefault(s => s.Id == id);
        }
        
        public IEnumerable<Brand> GetBrands() => _context.Brands.Include(s => s.Products);
        public Brand GetBrand(int id)
        {
            return _context.Brands
                .Include(b => b.Products).FirstOrDefault(b => b.Id == id);
        }
        
        public IEnumerable<Product> GetProducts(IProductFilter productFilter = null, bool includes = false)
        {
            IQueryable<Product> query = (includes) 
                ? _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Section) 
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
            _logger.LogInformation($"Запрос SQL: {query.ToQueryString()}");
            return query;
        }
        public Product GetProductById(int id) => _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .SingleOrDefault(p => p.Id == id);

        public int AddProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));
            _context.Add(product);
            _context.SaveChanges();
            #region Лог
            _logger.LogInformation($"Товар {product.Id} {product.Name} успешно добавлен в базу данных");
            #endregion
            return product.Id;
        }

        public void UpdateProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));
            if (_context.Products.Local.Any(e => e == product)) 
                _context.Update(product);
            else
            {
                var origin = _context.Products.Find(product.Id);
                origin.Name = product.Name;
                origin.Order = product.Order;
                origin.SectionId = product.SectionId;
                origin.BrandId = product.BrandId;
                origin.Price = product.Price;
                origin.ImageUrl = product.ImageUrl;
                _context.Update(origin);
            }
            _context.SaveChanges();
            #region Лог
            _logger.LogInformation($"Товар {product.Id} {product.Name} успешно обновлен в базе данных");
            #endregion
        }

        public bool DeleteProduct(int id)
        {
            if (GetProductById(id) is not { } product)
            {
                #region Лог
                _logger.LogError($"Товар с идентификатором {id} не удалось удалить из базы данных");
                #endregion
                return false;
            }                
            _context.Remove(product);
            _context.SaveChanges();
            #region Лог
            _logger.LogInformation($"Товар {id} {product.Name} успешно удален из базы данных");
            #endregion
            return true;
        }
    }
}
