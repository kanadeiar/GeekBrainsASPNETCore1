using System.Threading.Tasks;
using WebStore.Domain.WebModels.Cart;

namespace WebStore.Interfaces.Services
{
    /// <summary>
    /// Сервис управления корзиной товаров
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="id"></param>
        void Add(int id);
        /// <summary>
        /// Удалить товар из корзины 1 штука
        /// </summary>
        /// <param name="id"></param>
        void Subtract(int id);
        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
        /// <summary>
        /// Очистить корзину
        /// </summary>
        void Clear();
        /// <summary>
        /// Получить вебмодель корзины
        /// </summary>
        /// <returns></returns>
        Task<CartWebModel> GetWebModel();
    }
}
