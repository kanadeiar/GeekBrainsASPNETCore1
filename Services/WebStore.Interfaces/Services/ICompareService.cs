using System.Threading.Tasks;
using WebStore.Domain.WebModels;

namespace WebStore.Interfaces.Services
{
    /// <summary> Сервис сравнения товаров </summary>
    public interface ICompareService
    {
        /// <summary> Добавление товара к сравнению </summary>
        /// <param name="id">идентификатор товара</param>
        /// <returns>Число товаров в сравнении</returns>
        int Add(int id);
        /// <summary> Добавление товара к сравнению и получение вебмодели для отображения вьюхи </summary>
        /// <param name="id">идентификатор товара</param>
        /// <returns>Число товаров больше двух и вебмодель</returns>
        (bool, CompareWebModel) AddAndGetWebModel(int id);
        /// <summary> Очистка товаров для сравнения </summary>
        void Clear();
        /// <summary> Получение вебмодели сравнения товаров </summary>
        /// <returns>вебмодель</returns>
        Task<CompareWebModel> GetWebModel();
    }
}