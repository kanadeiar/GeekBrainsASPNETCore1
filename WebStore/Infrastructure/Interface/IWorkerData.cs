using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Infrastructure.Interface
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
