using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Prodcast;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class ProdcastController : SiteBaseController
    {
        private readonly IProdcastService _prodcastService;
        private readonly UserManager<IdentityUser> _userManager;
        public ProdcastController(IProdcastService prodcastService, UserManager<IdentityUser> userManager)
        {
            _prodcastService = prodcastService;
            _userManager = userManager;
        }

        [HttpGet("/prodcasts")]
        public async Task<ActionResult> Index()
        {
            var model = new ProdcastPageDto
            {
                Sliders = await _prodcastService.GetInSliderProdcasts(),
                NewestProdcasts = await _prodcastService.GetNewestProdcasts(25),
                PopularProdcasts = await _prodcastService.GetPopularProdcasts(25),
                ThisMonthProdcasts = await _prodcastService.GetProdcastsByStartDate(30, 16),
                ThisWeekProdcasts = await _prodcastService.GetProdcastsByStartDate(7, 16),
                ThisDayProdcasts = await _prodcastService.GetProdcastsByStartDate(1, 16),
            };
            return View(model);
        }

        [HttpGet("/prodcast/{id}")]
        public async Task<IActionResult> ShowProdcast(long id)
        {
            var prodcast = await _prodcastService.GetProdcastForSiteBy(id);
            if (prodcast == null) return NotFound();
            await _prodcastService.AddPlaysProdcast(prodcast);
            var model = new ShowProdcastPageDto
            {
                Prodcast = prodcast,
                RelatedProdcasts = await _prodcastService.GetRelatedProdcast(prodcast.DjId,id),
            };
            return View(model);
        }
        [HttpGet("/dj/{id}")]
        public async Task<IActionResult> ShowDj(long id)
        {
            var dj = await _prodcastService.GetDjForSiteBy(id);
            if (dj == null) return NotFound();
            return View(dj);
        }
        [HttpGet("/prodcast/all")]
        public async Task<IActionResult> ShowAllProdcast()
        {
            return View(await _prodcastService.GetAllProdcastForSite());
        }
        [HttpPost("/AddProdcastLike/{id}")]
        public async Task<IActionResult> AddProdcastLike(int id)
        {
            if (await _prodcastService.AddLikeProdcast(id, _userManager.GetUserId(User))) return Json(true);
            else return Json(false);
        }

    }
}
