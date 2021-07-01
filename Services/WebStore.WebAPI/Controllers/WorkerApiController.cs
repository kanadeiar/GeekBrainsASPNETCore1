using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    /// <summary> Апи контроллер сотрудников </summary>
    [Route(WebAPIInfo.Worker), ApiController]
    public class PersonApiController : ControllerBase
    {
        private readonly IWorkerData _workerData;
        public PersonApiController(IWorkerData workerData)
        {
            _workerData = workerData;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_workerData.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_workerData.Get(id));
        }

        [HttpPost]
        public IActionResult Add(Worker worker)
        {
            var id = _workerData.Add(worker);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(Worker worker)
        {
            _workerData.Update(worker);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _workerData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
