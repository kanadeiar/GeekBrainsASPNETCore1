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
        public int CountChildren { get; set; }
    }
}
