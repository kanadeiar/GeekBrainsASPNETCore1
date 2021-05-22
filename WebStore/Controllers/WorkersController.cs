using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class WorkersController : Controller
    {
        private static readonly IEnumerable<Worker> __Workers = Worker.GetWorkers;

        public IActionResult Index()
        {
            return View(__Workers);
        }
    }
}
