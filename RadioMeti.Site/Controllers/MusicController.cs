using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Music;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Utilities.Identity;

namespace RadioMeti.Site.Controllers
{
    public class MusicController : SiteBaseController
    {
        private readonly IMusicService _musicService;
        private readonly IArtistService _artistService;
        private UserManager<IdentityUser> _userManager;
        public MusicController(IMusicService musicService, IArtistService artistService, UserManager<IdentityUser>userManager)
        {
            _musicService = musicService;
            _artistService = artistService;
            _userManager = userManager;
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
        [HttpGet("/album/{albumId}")]
        public async Task<IActionResult> ShowAlbum(long albumId, long itemId)
        {
            var album = await _musicService.GetAlbumForSiteBy(albumId);
            if (album == null) return NotFound();
            var music = await _musicService.GetMusicForSiteBy(itemId);
            if (music == null)
            {
                return View(new ShowAlbumPageDto { Album = album });
            }
            await _musicService.AddPlaysMusic(music);
            var model = new ShowAlbumPageDto
            {
                Music = music,
                Album = album
            };
            return View(model);
        }
        [HttpGet("/artist/{id}")]
        public async Task<IActionResult> ShowArtist(long id)
        {
            var artist = await _artistService.GetArtistForSiteBy(id);
            if (artist== null) return NotFound();
            return View(artist);
        }
        [HttpGet("/music/all")]
        public async Task<IActionResult> ShowAllMusics()
        {
            return View(await _musicService.GetAllMusicsForSite());
        }
        [HttpGet("/album/all")]
        public async Task<IActionResult> ShowAllAlbums()
        {
            return View(await _musicService.GetAllAlbumsForSite());
        }
        [HttpPost("/AddMovieLike/{id}")]
        public async Task<IActionResult> AddMusicLike(int id)
        {
            if (await _musicService.AddLikeMusic(id,_userManager.GetUserId(User))) return Json(true);
            else return Json(false);
        }

    }
}
