using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.WebModels;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsWebModel();

            return View(model);
        }
    }
}
