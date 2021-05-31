﻿using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Details(int id)
        {
            var worker = __Workers.First(w => w.Id == id);

            if (worker is null)
                return NotFound();

            return View(worker);
        }
    }
}
