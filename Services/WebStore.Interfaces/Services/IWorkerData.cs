using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IWorkerData
    {
        /// <summary> Получить всех работников </summary>
        Task<IEnumerable<Worker>> GetAll();
        /// <summary> Получить одного работника </summary>
        Task<Worker> Get(int id);
        /// <summary> Добавить нового работника </summary>
        Task<int> Add(Worker worker);
        /// <summary> Обновить данные по работнику </summary>
        Task Update(Worker worker);
        /// <summary> Удалить работника </summary>
        Task<bool> Delete(int id);
    }
}
