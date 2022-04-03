using Microsoft.AspNetCore.Mvc;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class ProdcastController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
