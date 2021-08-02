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

        public async Task<IActionResult> GetHtml(int? id, string message, int Delay = 1000)
        {
            await Task.Delay(Delay);

            return PartialView("Partial/_AjaxDataViewPartial", 
                new AjaxDataWebModel
                {
                    Id = id ?? 1,
                    Message = message,
                });
        }
    }
}
