using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    /// <summary> Хранение данных по работникам в базе данных </summary>
    public class DatabaseWorkerData : IWorkerData
    {
        private readonly WebStoreContext _context;
        private readonly ILogger<DatabaseProductData> _logger;
        public DatabaseWorkerData(WebStoreContext context, ILogger<DatabaseProductData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Worker> GetAll() => _context.Workers;

        public Worker Get(int id) => _context.Workers.Find(id);

        public int Add(Worker worker)
        {
            if (worker is null)
                throw new ArgumentNullException(nameof(worker));
            _context.Workers.Add(worker);
            _context.SaveChanges();
            #region Лог
            _logger.LogInformation($"Пользователь {worker.LastName} {worker.FirstName} {worker.Patronymic} успешно добавлен в базу данных");
            #endregion
            return worker.Id;
        }

        public void Update(Worker worker)
        {
            if (worker is null)
                throw new ArgumentNullException(nameof(worker));
            if (_context.Workers.Local.Any(e => e == worker) == false) 
            {
                var origin = _context.Workers.Find(worker.Id);
                origin.LastName = worker.LastName;
                origin.FirstName = worker.FirstName;
                origin.Patronymic = worker.Patronymic;
                origin.Age = worker.Age;
                origin.Birthday = worker.Birthday;
                origin.CountChildren = worker.CountChildren;
                origin.EmploymentDate = worker.EmploymentDate;
                _context.Entry(origin).State = EntityState.Modified;
            }
            else
                _context.Entry(worker).State = EntityState.Modified;
            _context.SaveChanges();
            #region Лог
            _logger.LogInformation($"Пользователь {worker.LastName} {worker.FirstName} {worker.Patronymic} успешно обновлен в базе данных");
            #endregion
        }

        public bool Delete(int id)
        {
            if (Get(id) is not { } item) 
                return false;
            _context.Workers.Remove(item);
            _context.SaveChanges();
            #region Лог
            _logger.LogInformation($"Пользователь {item.LastName} {item.FirstName} {item.Patronymic} успешно удален из базы данных");
            #endregion
            return true;
        }
    }
}
