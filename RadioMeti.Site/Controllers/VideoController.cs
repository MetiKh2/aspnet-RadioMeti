using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Video;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
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
    }
}
