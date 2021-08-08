using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Identity;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrators)]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendEmail()
        {
            var model = new SendEmailMessageWebModel
            {
                Subject = "Email сообщение пользователю",
                Body = "Тестовое Email сообщение пользователю",
            };
            ViewBag.Success = false;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(SendEmailMessageWebModel model, [FromServices] ILogger<SendEmailService> logger)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Success = false;
                return View(model);
            }

            ISendEmailService service = new SendEmailService(model.NameFrom, model.MailFrom, model.Address, model.Port,
                new NetworkCredential(model.Login, model.Password), logger);
            await service.SendEmailAsync(model.NameTo, model.MailTo, model.Subject, model.Body);

            ViewBag.Success = true;
            var newModel = new SendEmailMessageWebModel
            {
                Subject = "Email сообщение пользователю",
                Body = "Тестовое Email сообщение пользователю",
            };
            return View(newModel);
        }
    }
}
