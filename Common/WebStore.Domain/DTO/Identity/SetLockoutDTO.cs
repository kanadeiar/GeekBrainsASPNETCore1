using System;
using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    public class SetLockoutDTO : UserDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
