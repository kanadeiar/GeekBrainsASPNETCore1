using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.WebModels.Account
{
    /// <summary> Веб модель регистрации </summary>
    public class RegisterWebModel
    {
        /// <summary> Имя пользователя </summary>
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        /// <summary> Пароль </summary>
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary> Подтверждения пароля </summary>
        [Required]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
