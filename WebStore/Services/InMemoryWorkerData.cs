using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interface;
using WebStore.Models;

namespace WebStore.Services
{
    /// <summary> Хранилище в оперативной памяти </summary>
    public class InMemoryWorkerData : IWorkerData
    {
        private readonly ICollection<Worker> _Workers = Worker.GetTestWorkers;
        private int maxId;
        public InMemoryWorkerData()
        {
            maxId = _Workers.Max(w => w.Id);
        }

        public IEnumerable<Worker> GetAll() => _Workers;
        public Worker Get(int id) => _Workers.SingleOrDefault(w => w.Id == id);
        public int Add(Worker worker)
        {
            if (worker is null)
                throw new ArgumentNullException(nameof(worker));
            if (_Workers.Contains(worker))
                return worker.Id;
            worker.Id = ++maxId;
            _Workers.Add(worker);
            return worker.Id;
        }
        public void Update(Worker worker)
        {
            if (worker is null)
                throw new ArgumentNullException(nameof(worker));
            if (_Workers.Contains(worker))
                return;
            var item = Get(worker.Id);
            if (item is null) return;
            item.LastName = worker.LastName;
            item.FirstName = worker.FirstName;
            item.Patronymic = worker.Patronymic;
            item.Age = worker.Age;
            item.Birthday = worker.Birthday;
            item.CountChildren = worker.CountChildren;
            item.EmploymentDate = worker.EmploymentDate;
        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return false;
            return _Workers.Remove(item);
        }
    }
}
