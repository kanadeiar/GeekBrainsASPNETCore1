using System.Collections.Generic;
using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity.Base
{
    /// <summary> Права пользователей </summary>
    public abstract class ClaimDTO : UserDTO
    {
        /// <summary> Права </summary>
        public IEnumerable<Claim> Cliams { get; set; }
    }
}
