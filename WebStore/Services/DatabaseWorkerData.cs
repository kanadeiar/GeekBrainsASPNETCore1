using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services
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
            _context.Workers.Add(worker);
            _context.SaveChanges();
            return worker.Id;
        }

        public void Update(Worker worker)
        {
            _context.Entry(worker).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item is null) return false;
            _context.Workers.Remove(item);
            _context.SaveChanges();
            return true;
        }
    }
}
