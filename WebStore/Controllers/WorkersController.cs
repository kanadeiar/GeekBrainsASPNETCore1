using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interface;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class WorkersController : Controller
    {
        private readonly IWorkerData _Workers;

        public WorkersController(IWorkerData workerData)
        {
            _Workers = workerData;
        }

        public IActionResult Index()
        {
            return View(_Workers.GetAll());
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get(id);

            if (worker is null)
                return NotFound();

            var model = new WorkerViewModel
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                Age = worker.Age,
                Birthday = worker.Birthday,
                EmploymentDate = worker.EmploymentDate,
                CountChildren = worker.CountChildren,
            };
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new WorkerViewModel());
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get((int)id);
            if (worker is null)
                return NotFound();

            var model = new WorkerViewModel
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                Age = worker.Age,
                Birthday = worker.Birthday,
                EmploymentDate = worker.EmploymentDate,
                CountChildren = worker.CountChildren,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(WorkerViewModel model)
        {            
            if (model is null)
                return BadRequest();
            if (model.FirstName == "Андрей")
                ModelState.AddModelError(nameof(model.FirstName), "Андрей - плохое имя!");
            if (model.LastName == "Иванов" && model.FirstName == "Иван" && model.Patronymic == "Иванович")
                ModelState.AddModelError(string.Empty, "Нельзя иметь фамилию имя и отчество Иванов Иван Иванович");
            if (!ModelState.IsValid)
                return View(model);

            var worker = new Worker
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Birthday = model.Birthday,
                EmploymentDate = model.EmploymentDate,
                CountChildren = model.CountChildren,
            };
            if (worker.Id == 0)
                _Workers.Add(worker);
            else
                _Workers.Update(worker);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get(id);
            if (worker is null)
                return NotFound();

            var model = new WorkerViewModel
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                Age = worker.Age,
                Birthday = worker.Birthday,
                EmploymentDate = worker.EmploymentDate,
                CountChildren = worker.CountChildren,
            };
            return View(model);
        }
        
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0) 
                return BadRequest();
            if (!_Workers.Delete(id))
                return BadRequest();

            return RedirectToAction("Index");
        }
    }
}
