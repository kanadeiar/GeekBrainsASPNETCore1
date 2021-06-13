using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Controllers
{
    public class WorkersController : Controller
    {
        private readonly IWorkerData _Workers;
        private readonly ILogger<WorkersController> _logger;

        public WorkersController(IWorkerData workerData, ILogger<WorkersController> logger)
        {
            _Workers = workerData;
            _logger = logger;
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

            var model = new WorkerWebModel()
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
                return View(new WorkerWebModel());
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get((int)id);
            if (worker is null)
                return NotFound();

            var model = new WorkerWebModel
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
        public IActionResult Edit(WorkerWebModel model)
        {            
            if (model is null)
                return BadRequest();
            if (model.FirstName == "Андрей")
                ModelState.AddModelError(nameof(model.FirstName), "Андрей - плохое имя для работника!");
            if (model.LastName == "Иванов" && model.FirstName == "Иван" && model.Patronymic == "Иванович")
                ModelState.AddModelError(string.Empty, "Нельзя иметь фамилию имя и отчество Иванов Иван Иванович");
            if (!ModelState.IsValid)
                return View(model);
            _logger.LogDebug($"Начало редактирования сотрудника id={model.Id}");

            var worker = _Workers.Get(model.Id) ?? new Worker();

            worker.FirstName = model.FirstName;
            worker.LastName = model.LastName;
            worker.Patronymic = model.Patronymic;
            worker.Age = model.Age;
            worker.Birthday = model.Birthday;
            worker.EmploymentDate = model.EmploymentDate;
            worker.CountChildren = model.CountChildren;

            if (worker.Id == 0)
                _Workers.Add(worker);
            else
                _Workers.Update(worker);
            _logger.LogDebug($"Редактирование сотрудника id={model.Id} завершено");

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get(id);
            if (worker is null)
                return NotFound();

            var model = new WorkerWebModel
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
            _logger.LogDebug($"Начало удаления сотрудника id={id}");

            if (!_Workers.Delete(id))
                return BadRequest();
            _logger.LogDebug($"Окончание успешного удаления сотрудника id={id}");

            return RedirectToAction("Index");
        }
    }
}
