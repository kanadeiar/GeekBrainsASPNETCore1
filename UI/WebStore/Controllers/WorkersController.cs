using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class WorkersController : Controller
    {
        private readonly IWorkerData _Workers;
        private readonly ILogger<WorkersController> _logger;
        private readonly Mapper _mapperWorkerToWeb = 
            new (new MapperConfiguration(c => c.CreateMap<Worker, EditWorkerWebModel>()));
        private readonly Mapper _mapperWorkerFromWeb =
            new(new MapperConfiguration(c => c.CreateMap<EditWorkerWebModel, Worker>()));

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

            return View(_mapperWorkerToWeb.Map<EditWorkerWebModel>(worker));
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EditWorkerWebModel());
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get((int)id);
            if (worker is null)
                return NotFound();
            
            return View(_mapperWorkerToWeb.Map<EditWorkerWebModel>(worker));
        }

        [HttpPost, Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EditWorkerWebModel model)
        {            
            if (model is null)
                return BadRequest();
            if (model.FirstName == "Андрей")
                ModelState.AddModelError(nameof(model.FirstName), "Андрей - плохое имя для работника!");
            if (model.LastName == "Иванов" && model.FirstName == "Иван" && model.Patronymic == "Иванович")
                ModelState.AddModelError(string.Empty, "Нельзя иметь фамилию имя и отчество Иванов Иван Иванович");
            if (!ModelState.IsValid)
                return View(model);
            #region Лог
            _logger.LogInformation($"Начало редактирования сотрудника id={model.Id}");
            #endregion
            var worker = _mapperWorkerFromWeb.Map<Worker>(model);

            if (worker.Id == 0)
                _Workers.Add(worker);
            else
                _Workers.Update(worker);
            #region Лог
            _logger.LogInformation($"Редактирование сотрудника id={model.Id} завершено");
            #endregion
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id <= 0) 
                return BadRequest();

            var worker = _Workers.Get(id);
            if (worker is null)
                return NotFound();

            return View(_mapperWorkerToWeb.Map<EditWorkerWebModel>(worker));
        }
        
        [HttpPost, Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0) 
                return BadRequest();
            #region Лог
            _logger.LogDebug($"Начало удаления сотрудника id={id}");
            #endregion
            if (!_Workers.Delete(id))
                return BadRequest();
            #region Лог
            _logger.LogDebug($"Окончание успешного удаления сотрудника id={id}");
            #endregion
            return RedirectToAction("Index");
        }
    }
}
