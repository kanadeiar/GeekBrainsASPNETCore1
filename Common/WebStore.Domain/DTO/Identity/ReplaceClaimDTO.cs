using System.Security.Claims;
using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    /// <summary> Замена прав у пользователя </summary>
    public class ReplaceClaimDTO : UserDTO
    {
        /// <summary> Старое право </summary>
        public Claim Claim { get; set; }
        /// <summary> Новое право </summary>
        public Claim NewClaim { get; set; }
    }
}
