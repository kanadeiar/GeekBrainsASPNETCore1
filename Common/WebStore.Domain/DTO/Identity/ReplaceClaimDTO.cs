using System.Security.Claims;
using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    public class ReplaceClaimDTO : UserDTO
    {
        public Claim Claim { get; set; }
        public Claim NewClaim { get; set; }
    }
}
