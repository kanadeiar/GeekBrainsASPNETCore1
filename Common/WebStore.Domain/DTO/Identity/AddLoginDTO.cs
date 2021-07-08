using Microsoft.AspNetCore.Identity;
using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    /// <summary> Добавление логина пользователю </summary>
    public class AddLoginDTO : UserDTO
    {
        /// <summary> Информация по логину пользователя </summary>
        public UserLoginInfo UserLoginInfo { get; set; }
    }
}
