using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    /// <summary> Установка хеш пароля пользователя </summary>
    public class PasswordHashDTO : UserDTO
    {
        /// <summary> Хеш пароля пользователя </summary>
        public string Hash { get; set; }
    }
}
