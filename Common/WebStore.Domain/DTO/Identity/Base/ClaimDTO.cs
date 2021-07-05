using System.Collections.Generic;
using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity.Base
{
    public abstract class ClaimDTO : UserDTO
    {
        public IEnumerable<Claim> Cliams { get; set; }
    }
}
