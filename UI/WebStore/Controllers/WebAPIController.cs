using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.WebAPI;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService _valuesService;
        public WebAPIController(IValuesService valuesService)
        {
            _valuesService = valuesService;
        }
        public IActionResult Index()
        {
            var values = _valuesService.GetAll();
            return View(values);
        }
    }
}
