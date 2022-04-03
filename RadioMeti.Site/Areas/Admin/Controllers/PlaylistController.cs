using Microsoft.AspNetCore.Mvc;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class PlaylistController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
