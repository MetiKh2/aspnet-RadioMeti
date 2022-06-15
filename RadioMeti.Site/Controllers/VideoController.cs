using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Video;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        private readonly UserManager<IdentityUser> _userManager;
        public VideoController(IVideoService videoService, UserManager<IdentityUser> userManager)
        {
            _videoService = videoService;
            _userManager = userManager;
        }

        [HttpGet("/videos")]
        public async Task<ActionResult> Index()
        {
            var model = new VideoPageDto
            {
                Sliders = await _videoService.GetInSliderVideos(),
                NewestVideos = await _videoService.GetNewestVideos(25),
                PopularVideos = await _videoService.GetPopularVideos(25),
                ThisMonthVideos = await _videoService.GetVideosByStartDate(30, 16),
                ThisWeekVideos = await _videoService.GetVideosByStartDate(7, 16),
                ThisDayVideos = await _videoService.GetVideosByStartDate(1, 16),
            };
            return View(model);
        }

        [HttpGet("/video/{id}")]
        public async Task<IActionResult> ShowVideo(long id)
        {
            var video = await _videoService.GetVideoForSiteBy(id);
            if (video == null) return NotFound();
            await _videoService.AddPlaysVideo(video);
            var model = new ShowVideoPageDto
            {
                Video = video,
                RelatedVideos = await _videoService.GetRelatedVideos(video),
            };
            return View(model);
        }
        [HttpGet("/video/all")]
        public async Task<IActionResult> ShowAllVideos()
        {
            return View(await _videoService.GetAllVideosForSite());
        }
        [HttpPost("/AddVideoLike/{id}")]
        public async Task<IActionResult> AddProdcastLike(int id)
        {
            if (await _videoService.AddLikeVideo(id, _userManager.GetUserId(User))) return Json(true);
            else return Json(false);
        }
    }
}
