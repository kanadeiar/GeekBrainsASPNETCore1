using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers
{
    /// <summary> Тестовые значения </summary>
    [Route(WebAPIInfo.ApiValue), ApiController]
    public class ValuesController : ControllerBase
    {
        private static List<string> __testList = Enumerable
            .Range(1, 20)
            .Select(i => $"Тестовое значение № {i}")
            .ToList();

        /// <summary> Получить все тестовые значения </summary>
        /// <returns>Тестовые значения</returns>
        [HttpGet]
        public IActionResult Get() => Ok(__testList);

        /// <summary> Количество тестовых значений </summary>
        /// <returns>Количество значений</returns>
        [HttpGet("count")]
        public IActionResult GetCount() => Ok(__testList.Count);

        /// <summary> Получить одно значение по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Тестовое значение</returns>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id > __testList.Count)
                return NotFound();
            return Ok(__testList[id]);
        }

        /// <summary> Добавить одно тестовое значение </summary>
        /// <param name="str">Значение</param>
        /// <returns>Результат операции</returns>
        [HttpGet("add")]
        public IActionResult Add(string str)
        {
            __testList.Add(str);
            return Ok();
        }

        /// <summary> Обновить одно тестовое значение </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="str">Значение</param>
        /// <returns>Результат операции</returns>
        [HttpPut("{id:int}")]
        public IActionResult Replace(int id, string str)
        {
            if (id < 0)
                return BadRequest();
            if (id > __testList.Count)
                return NotFound();
            __testList[id] = str;
            return Ok();
        }

        /// <summary> Удалить одно тестовое значение </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Результат операции</returns>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id > __testList.Count)
                return NotFound();
            __testList.RemoveAt(id);
            return Ok();
        }
    }
}
