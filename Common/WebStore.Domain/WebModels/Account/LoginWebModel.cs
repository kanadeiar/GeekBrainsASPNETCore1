using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.WebModels.Account
{
    /// <summary> Веб модель входа в систему </summary>
    public class LoginWebModel
    {
        /// <summary> Имя пользователя </summary>
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        /// <summary> Пароль пользователя </summary>
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary> Запомнить этого пользователя </summary>
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
        /// <summary> Возвращение на страницу </summary>
        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
