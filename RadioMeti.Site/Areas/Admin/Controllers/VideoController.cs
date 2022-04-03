using Microsoft.AspNetCore.Mvc;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class VideoController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
