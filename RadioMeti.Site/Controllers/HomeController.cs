using Microsoft.AspNetCore.Mvc;
using RadioMeti.Site.Models;
using System.Diagnostics;

namespace RadioMeti.Site.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}