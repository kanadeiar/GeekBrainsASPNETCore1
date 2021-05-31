using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class WorkersController : Controller
    {
        private static readonly IList<Worker> __Workers = Worker.GetWorkers;

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

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new Worker());

            var worker = __Workers.FirstOrDefault(w => w.Id == id);
            if (worker is null)
                return NotFound();

            var model = new Worker
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                Age = worker.Age,
                Birthday = worker.Birthday,
                EmploymentDate = worker.EmploymentDate,
                CountClildren = worker.CountClildren,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Worker model)
        {
            var worker = new Worker
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Birthday = model.Birthday,
                EmploymentDate = model.EmploymentDate,
                CountClildren = model.CountClildren,
            };
            if (worker.Id == 0)
                __Workers.Add(worker);
            else
                __Workers[__Workers.IndexOf(__Workers.FirstOrDefault(w => w.Id == worker.Id))] = worker;

            return RedirectToAction("Index");
        }
    }
}
