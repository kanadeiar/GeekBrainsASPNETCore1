using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DatabaseWorkerData : IWorkerData
    {
        public IEnumerable<Worker> GetAll()
        {
            throw new NotImplementedException();
        }

        public Worker Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Add(Worker worker)
        {
            throw new NotImplementedException();
        }

        public void Update(Worker worker)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
