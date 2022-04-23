using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Music;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class MusicController : SiteBaseController
    {
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpGet("/musics")]
        public async Task<ActionResult> Index()
        {
            var model = new MusicsPageDto {
                Sliders = await _musicService.GetInSliderMusics(),
                NewestMusics = await _musicService.GetNewestMusics(25),
                PopularMusics = await _musicService.GetPopularMusics(25),
                ThisMonthMusics = await _musicService.GetMusicsByStartDate(30, 16),
                ThisWeekMusics = await _musicService.GetMusicsByStartDate(7, 16),
                ThisDayMusics = await _musicService.GetMusicsByStartDate(1, 16),
                LastAlbums = await _musicService.GetLastAlbums(10)
            };
            return View(model);
        }

        [HttpGet("/music/{id}")]
        public async Task<IActionResult> ShowMusic(long id)
        {
            var music = await _musicService.GetMusicForSiteBy(id);
            if (music == null) return NotFound();
           await _musicService.AddPlaysMusic(music);
            var model = new ShowMusicPageDto { 
            Music = music,
            RelatedMusics=await _musicService.GetRelatedMusics(music), 
            };
            return View(model);
        }

    }
}
