using WebStore.Domain.DTO.Identity.Base;

namespace WebStore.Domain.DTO.Identity
{
    public class PasswordHashDTO : UserDTO
    {
        public string Hash { get; set; }
    }
}
