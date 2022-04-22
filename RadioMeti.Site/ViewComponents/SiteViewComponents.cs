using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Slider;

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

public class SiteFooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("SiteFooter");
    }
}

#endregion
#region Slider

public class SiteSliderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<SiteSliderDto> model)
    {
        return View("SiteSlider",model);
    }
}

#endregion