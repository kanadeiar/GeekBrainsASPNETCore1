using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface IWorkerData
    {
        /// <summary> Получить всех работников </summary>
        IEnumerable<Worker> GetAll();
        /// <summary> Получить одного работника </summary>
        Worker Get(int id);
        /// <summary> Добавить нового работника </summary>
        int Add(Worker worker);
        /// <summary> Обновить данные по работнику </summary>
        void Update(Worker worker);
        /// <summary> Удалить работника </summary>
        bool Delete(int id);
    }
}
