using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    /// <summary> Хранилище в оперативной памяти </summary>
    public class InMemoryWorkerData : IWorkerData
    {
        private readonly ILogger<InMemoryWorkerData> _logger;
        private readonly List<Worker> _Workers;
        private int maxId;

        public InMemoryWorkerData(TestData testData, ILogger<InMemoryWorkerData> logger)
        {
            _logger = logger;
            _Workers = testData.GetTestWorkers.ToList();
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
            _logger.LogInformation($"Сотрудник id={worker.Id} добавлен");
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
            _logger.LogInformation($"Сотрудник id={worker.Id} отредактирован");
        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item is null)
            {
                _logger.LogWarning($"При удалении сотрудник id={id} не найден");
                return false;
            }
            var result = _Workers.Remove(item);
            if (result)
                _logger.LogInformation($"Сотрудник id={id} успешно удален");
            else
                _logger.LogError($"В процессе удаления сотрудник id={id} не найден");
            return result;
        }
    }
}
