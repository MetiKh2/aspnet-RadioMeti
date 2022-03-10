using Microsoft.AspNetCore.Mvc;

namespace RadioMeti.Site.ViewComponents;

#region Header

public class SiteHeaderViewComponent:ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("SiteHeader");
    }
}

#endregion

#region Footer

public class SiteFooterViewComponent:ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("SiteFooter");
    }
}

#endregion