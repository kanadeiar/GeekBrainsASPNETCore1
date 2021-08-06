using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Веб модель редактирования товара </summary>
    public class EditProductWebModel
    {
        /// <summary> Идентификатор </summary>
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        /// <summary> Название товара </summary>
        [Display(Name = "Название товара")]
        [Required(ErrorMessage = "Нужно ввести название товара")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длинна названия должна быть от 2 до 200 символов")]
        public string Name { get; set; }
        /// <summary> Сортировка товара </summary>
        [Display(Name = "Сортировка")]
        [Range(0, 90000, ErrorMessage = "Пожалуйста, только без отрицательных значений")]
        public int Order { get; set; }
        /// <summary> Категория товара </summary>
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Нужно обязательно выбрать категорию товара")]
        public int SectionId { get; set; }
        /// <summary> Категория товара </summary>
        [Display(Name = "Категория")]
        public string SectionName { get; set; }
        /// <summary> Бренд товара </summary>
        [Display(Name = "Бренд")]
        public int? BrandId { get; set; }
        /// <summary> Название бренда </summary>
        [Display(Name = "Бренд")]
        public string BrandName { get; set; }
        /// <summary> Путь к файлу изображения товара </summary>
        [Display(Name = "Изображение товара")]
        public string ImageUrl { get; set; }
        /// <summary> Стоимость товара </summary>
        [Display(Name = "Стоимость")]
        [Range(0.0, 90000.0, ErrorMessage = "Стоимость может колебаться от 0 до 90000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        /// <summary> Ключевые слова этого товара </summary>
        [Display(Name = "Ключевые слова")]
        public ICollection<TagWebModel> Tags { get; set; }
    }
}
