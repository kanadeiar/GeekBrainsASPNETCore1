using WebStore.Domain.Identity;

namespace WebStore.Domain.DTO.Identity.Base
{
    /// <summary> Пользователь </summary>
    public abstract class UserDTO
    {
        /// <summary> Пользователь </summary>
        public User User { get; set; }
    }
}
