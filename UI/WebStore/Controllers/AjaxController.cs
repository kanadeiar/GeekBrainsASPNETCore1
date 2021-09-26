using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.WebModels.Ajax;

namespace WebStore.Controllers
{
    public class AjaxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHtml(int? id, string message, int delay = 1000)
        {
            await Task.Delay(delay);

            return PartialView("Partial/_AjaxDataViewPartial",
                new AjaxDataWebModel
                {
                    Id = id ?? 1,
                    Message = message,
                    ServerTime = DateTime.Now,
                });
        }

        public async Task<IActionResult> GetJson(int? id, string message, int delay = 1000)
        {
            await Task.Delay(delay);

            return Json(new
            {
                id = id ?? 1,
                message = $"Ответное: id = {id ?? 0} message = {message ?? "<null>"}",
                ServerTime = DateTime.Now,
            });
        }

        public IActionResult SignalRChat()
        {
            return View();
        }
    }
}
