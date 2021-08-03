using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class BlazorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
