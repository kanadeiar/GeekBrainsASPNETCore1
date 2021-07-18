using System;
using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    /// <summary> Один работник </summary>
    public class Worker : Entity
    {
        /// <summary> Имя </summary>
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        /// <summary> Фамилия </summary>
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        /// <summary> Отчество </summary>
        [Required, MaxLength(100)]
        public string Patronymic { get; set; }
        /// <summary> Возраст </summary>
        public int Age { get; set; }
        /// <summary> День рождения </summary>
        public DateTime Birthday { get; set; }
        /// <summary> Дата трудоустройства </summary>
        public DateTime EmploymentDate { get; set; }
        /// <summary> Количество детей, штук </summary>
        public int CountChildren { get; set; }
    }
}
