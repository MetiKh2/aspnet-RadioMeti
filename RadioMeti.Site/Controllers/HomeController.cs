using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace RadioMeti.Site.Controllers
{
    public class HomeController : SiteBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}