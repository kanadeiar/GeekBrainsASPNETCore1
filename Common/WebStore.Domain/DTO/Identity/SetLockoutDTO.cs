using System;
using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    /// <summary> Установка даты окончания блокировки </summary>
    public class SetLockoutDTO : UserDTO
    {
        /// <summary> Окончание блокировки пользователя </summary>
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
