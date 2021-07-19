using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    /// <summary> Апи контроллер сотрудников </summary>
    [Route(WebAPIInfo.ApiWorker), ApiController]
    public class PersonApiController : ControllerBase
    {
        private readonly IWorkerData _workerData;
        /// <summary> Конструктор </summary>
        public PersonApiController(IWorkerData workerData)
        {
            _workerData = workerData;
        }

        /// <summary> Получить всех работников </summary>
        /// <returns>Все работники</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _workerData.GetAll());
        }

        /// <summary> Получить работника по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Работник</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _workerData.Get(id));
        }

        /// <summary> Добавить нового работника </summary>
        /// <param name="worker">Работник</param>
        /// <returns>Идентификатор добавленного работника</returns>
        [HttpPost]
        public async Task<IActionResult> Add(Worker worker)
        {
            var id = await _workerData.Add(worker);
            return Ok(id);
        }

        /// <summary> Обновить сведения работника </summary>
        /// <param name="worker">Работник</param>
        /// <returns>Результат операции</returns>
        [HttpPut]
        public async Task<IActionResult> Update(Worker worker)
        {
            await _workerData.Update(worker);
            return Ok();
        }

        /// <summary> Удалить работника </summary>
        /// <param name="id">Идентификатор работника</param>
        /// <returns>Результат операции</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _workerData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
