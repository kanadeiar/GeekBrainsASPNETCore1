using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIInfo.Values), ApiController]
    public class ValuesController : ControllerBase
    {
        private static List<string> __testList = Enumerable
            .Range(1, 20)
            .Select(i => $"Тестовое значение № {i}")
            .ToList();

        [HttpGet]
        public IActionResult Get() => Ok(__testList);

        [HttpGet("count")]
        public IActionResult GetCount() => Ok(__testList.Count);

        //[HttpGet("id[[{id:int}]]")]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id > __testList.Count)
                return NotFound();
            return Ok(__testList[id]);
        }

        //[HttpPost("add")]
        //[HttpPost]
        [HttpGet("add")]
        public IActionResult Add(string str)
        {
            __testList.Add(str);
            return Ok();
        }

        //[HttpPut("edit/{id:int}")]
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
