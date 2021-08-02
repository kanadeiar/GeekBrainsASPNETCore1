using Microsoft.AspNetCore.Mvc;
using System;

namespace WebStore.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        [HttpGet("clear")]
        public void Clear()
        {
            Console.Clear();
        }

        [HttpGet("writeline")]
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
