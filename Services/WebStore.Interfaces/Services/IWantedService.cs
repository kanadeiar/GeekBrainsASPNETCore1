using System.Threading.Tasks;
using WebStore.Domain.WebModels;

namespace WebStore.Interfaces.Services
{
    /// <summary> Сервис желаемых товаров </summary>
    public interface IWantedService
    {
        /// <summary> Добавление товара в список желаемых </summary>
        /// <param name="id">Идентификатор</param>
        void Add(int id);
        /// <summary> Удаление товара из списка желаемых товаров </summary>
        /// <param name="id">Идентификатор</param>
        void Remove(int id);
        /// <summary> Очистка списка желаемых товаров </summary>
        void Clear();
        /// <summary> Получение вебмодели вьюхи желаемых товаров </summary>
        /// <returns>Вебмодель</returns>
        Task<WantedWebModel> GetWebModel();
    }
}