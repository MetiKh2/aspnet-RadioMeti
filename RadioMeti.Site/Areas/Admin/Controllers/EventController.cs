using Microsoft.AspNetCore.Mvc;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class EventController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
