using Microsoft.AspNetCore.Mvc;
using System;

namespace WebStore.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        [HttpGet]
        public void Clear()
        {
            Console.Clear();
        }

        [HttpGet]
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
