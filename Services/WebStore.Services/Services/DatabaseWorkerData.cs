using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Worker>> GetAll() => await _context.Workers.ToArrayAsync().ConfigureAwait(false);

        public async Task<Worker> Get(int id) => await _context.Workers.FindAsync(id).ConfigureAwait(false);

        public async Task<int> Add(Worker worker)
        {
            if (worker is null)
                throw new ArgumentNullException(nameof(worker));
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            #region Лог
            _logger.LogInformation("Пользователь {0} {1} {2} успешно добавлен в базу данных", worker.LastName, worker.FirstName, worker.Patronymic);
            #endregion
            return worker.Id;
        }

        public async Task Update(Worker worker)
        {
            if (worker is null)
                throw new ArgumentNullException(nameof(worker));
            if (_context.Workers.Local.Any(e => e == worker) == false) 
            {
                var origin = await _context.Workers.FindAsync(worker.Id).ConfigureAwait(false);
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
            await _context.SaveChangesAsync().ConfigureAwait(false);
            #region Лог
            _logger.LogInformation("Пользователь {0} {1} {2} успешно обновлен в базе данных", worker.LastName, worker.FirstName, worker.Patronymic);
            #endregion
        }

        public async Task<bool> Delete(int id)
        {
            if (await Get(id) is not { } item) 
                return false;
            _context.Workers.Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            #region Лог
            _logger.LogInformation("Пользователь {0} {1} {2} успешно удален из базы данных", item.LastName, item.FirstName, item.Patronymic);
            #endregion
            return true;
        }
    }
}
