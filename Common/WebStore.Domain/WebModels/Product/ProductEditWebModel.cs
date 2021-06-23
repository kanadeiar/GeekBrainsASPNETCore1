using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.WebModels.Product
{
    /// <summary> Веб модель редактирования товара </summary>
    public class ProductEditWebModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Название товара")]
        [Required(ErrorMessage = "Нужно ввести название товара")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длинна названия должна быть от 2 до 200 символов")]
        public string Name { get; set; }

        [Display(Name = "Сортировка")]
        [Range(0, 90000, ErrorMessage = "Пожалуйста, только без отрицательных значений")]
        public int Order { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Нужно обязательно выбрать категорию товара")]
        public int? SectionId { get; set; }
        [Display(Name = "Категория")]
        public string SectionName { get; set; }

        [Display(Name = "Бренд")]
        public int? BrandId { get; set; }

        [Display(Name = "Бренд")]
        public string BrandName { get; set; }

        [Display(Name = "Стоимость")]
        [Range(0.0, 90000.0, ErrorMessage = "Стоимость может колебаться от 0 до 90000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Изображение товара")]
        public string ImageUrl { get; set; }
    }
}
