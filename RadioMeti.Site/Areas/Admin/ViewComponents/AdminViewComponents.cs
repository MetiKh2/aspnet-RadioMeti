using Microsoft.AspNetCore.Mvc;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    #region Header

    public class AdminHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("AdminHeader");
        }
    }

    #endregion

  
}
