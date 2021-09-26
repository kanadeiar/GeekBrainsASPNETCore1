using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.Models.Interfaces;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        /// <summary> Все категории товаров </summary>
        Task<IEnumerable<Section>> GetSections();
        /// <summary> Одна категория </summary>
        /// <param name="id">Идентификатор</param>
        Task<Section> GetSection(int id);
        /// <summary> Все бренды </summary>
        Task<IEnumerable<Brand>> GetBrands();
        /// <summary> Один бренд </summary>
        /// <param name="id">Идентификатор</param>
        Task<Brand> GetBrand(int id);
        /// <summary> Все ключевые слова </summary>
        Task<IEnumerable<Tag>> GetTags();
        /// <summary> Одно ключевое слово </summary>
        /// <param name="id">Идентификатор</param>
        Task<Tag> GetTag(int id);
        /// <summary> Отобранные товары с фильтрацией по категориям и/или брендам и/или названию или либо с определенными идентификаторами </summary>
        Task<ProductPage> GetProducts(IProductFilter productFilter = null, bool includes = false);
        /// <summary> Получение одного товара по ид </summary>
        Task<Product> GetProductById(int id);
        /// <summary> Добавить товар </summary>
        /// <param name="product">Товар</param> <returns></returns>
        Task<int> AddProduct(Product product);
        /// <summary> Изменить товар </summary>
        /// <param name="product">Товар</param>
        Task UpdateProduct(Product product);
        /// <summary> Удалить товар </summary>
        /// <param name="id">Идентификатор</param> <returns>Удалось удалить или нет</returns>
        Task<bool> DeleteProduct(int id);
    }
}
