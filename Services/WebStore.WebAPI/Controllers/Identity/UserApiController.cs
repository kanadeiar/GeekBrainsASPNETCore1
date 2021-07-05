using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Dal.Context;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.ApiUser), ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserStore<User> _userStore;
        public UserApiController(WebStoreContext context)
        {
            _userStore = new UserStore<User>(context);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<User>> GetAllUsers() => 
            await _userStore.Users.ToArrayAsync();



    }
}
