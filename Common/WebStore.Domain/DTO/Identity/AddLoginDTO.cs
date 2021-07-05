using Microsoft.AspNetCore.Identity;
using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }
}
