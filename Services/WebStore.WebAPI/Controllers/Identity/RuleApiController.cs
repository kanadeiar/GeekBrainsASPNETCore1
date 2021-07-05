using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Dal.Context;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.ApiRole), ApiController]
    public class RuleApiController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;
        public RuleApiController(WebStoreContext context)
        {
            _roleStore = new RoleStore<Role>(context);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Role>> GetAllRoles() => 
            await _roleStore.Roles.ToArrayAsync();



    }
}
