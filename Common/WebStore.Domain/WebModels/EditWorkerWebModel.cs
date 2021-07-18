using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель редактирования данных работника </summary>
    public class EditWorkerWebModel 
    {
        /// <summary> Идентификатор </summary>
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary> Имя </summary>
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длинна имени должна быть от 2 до 200 символов")]
        [RegularExpression("([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Строка имела неверный формат (Либо русские, либо латинские символы, начиная с заглавной буквы)")]
        public string FirstName { get; set; }

        /// <summary> Фамилия </summary>
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длинна фамилии должна быть от 2 до 200 символов")]
        [RegularExpression("([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Строка имела неверный формат (Либо русские, либо латинские символы, начиная с заглавной буквы)")]
        public string LastName { get; set; }

        /// <summary> Отчество </summary>
        [Display(Name = "Отчество")]
        [StringLength(200, ErrorMessage = "Длинна отчества должна быть не более 200 символов")]
        [RegularExpression("([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Строка имела неверный формат (Либо русские, либо латинские символы, начиная с заглавной буквы)")]
        public string Patronymic { get; set; }

        /// <summary> Возраст </summary>
        [Display(Name = "Возраст")]
        [Range(18, 100, ErrorMessage = "Возраст должен быть от 18 до 100 лет")]
        public int Age { get; set; }

        /// <summary> День рождения </summary>
        [Display(Name = "День рождения")]
        public DateTime Birthday { get; set; }

        /// <summary> День трудоустройства </summary>
        [Display(Name = "День трудоустройства")]
        public DateTime EmploymentDate { get; set; }

        /// <summary> Детей, штук </summary>
        [Display(Name = "Количество детей, штук")]
        public int CountChildren { get; set; }
    }
}
