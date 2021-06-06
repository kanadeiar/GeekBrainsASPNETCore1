using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models.Base;

namespace WebStore.Models
{
    public class Worker : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int CountClildren { get; set; }

        #region Тестовые данные

        public static List<Worker> GetTestWorkers => Enumerable.Range(1, 10).Select(p => new Worker
        {
            Id = p,
            FirstName = $"Иван_{p}",
            LastName = $"Иванов_{p + 1}",
            Patronymic = $"Иванович_{p + 2}",
            Age = p + 20,
            Birthday = new DateTime(1980 + p, 1, 1),
            EmploymentDate = DateTime.Now.AddYears(- p).AddMonths(p),
            CountClildren = p,
        }).ToList();

        #endregion
    }
}
