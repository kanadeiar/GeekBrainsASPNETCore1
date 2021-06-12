using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface IWorkerData
    {
        IEnumerable<Worker> GetAll();
        Worker Get(int id);
        int Add(Worker worker);
        void Update(Worker worker);
        bool Delete(int id);
    }
}
