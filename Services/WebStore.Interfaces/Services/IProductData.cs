using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Domain.Models.Interfaces;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        /// <summary> Все категории товаров </summary>
        IEnumerable<Section> GetSections();
        /// <summary> Все бренды </summary>
        IEnumerable<Brand> GetBrands();
        /// <summary> Все товары с фильтрацией по категориям и/или брендам </summary>
        IEnumerable<Product> GetProducts(IProductFilter productFilter = null, bool includes = false);
        /// <summary> Категории товаров с включениями продуктов </summary>
        IEnumerable<Section> GetSectionsWithProducts();
        /// <summary> Бренды с включениями продуктов </summary>
        IEnumerable<Brand> GetBrandsWithProducts();
        /// <summary> Получение одного товара по ид </summary>
        Product GetProductById(int id);
        /// <summary> Добавить товар </summary>
        /// <param name="product">Товар</param> <returns></returns>
        int AddProduct(Product product);
        /// <summary> Изменить товар </summary>
        /// <param name="product">Товар</param>
        void UpdateProduct(Product product);
        /// <summary> Удалить товар </summary>
        /// <param name="id">Идентификатор</param> <returns>Удалось удалить или нет</returns>
        bool DeleteProduct(int id);
    }
}
