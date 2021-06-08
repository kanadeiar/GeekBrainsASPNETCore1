using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models;

namespace WebStore.Data
{
    public class TestData
    {
        #region Тестовые данные

        public List<Worker> GetTestWorkers => Enumerable.Range(1, 10).Select(p => new Worker
        {
            Id = p,
            FirstName = $"Иван_{p}",
            LastName = $"Иванов_{p + 1}",
            Patronymic = $"Иванович_{p + 2}",
            Age = p + 20,
            Birthday = new DateTime(1980 + p, 1, 1),
            EmploymentDate = DateTime.Now.AddYears(- p).AddMonths(p),
            CountChildren = p,
        }).ToList();

        #endregion
    }
}
