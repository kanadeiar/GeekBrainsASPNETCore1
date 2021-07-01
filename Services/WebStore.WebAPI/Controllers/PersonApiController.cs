using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIInfo.Persons), ApiController]
    public class PersonApiController : ControllerBase
    {
        private readonly IWorkerData _workerData;
        public PersonApiController(IWorkerData workerData)
        {
            _workerData = workerData;
        }




    }
}
