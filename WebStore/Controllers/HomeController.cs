using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogSingle()
        {
            return View();
        }





        #region Вспомогательные

        public IActionResult Error()
        {
            return View();
        }

        #endregion
    }
}
