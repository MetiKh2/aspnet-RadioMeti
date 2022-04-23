using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Playlist;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMusicService _musicService;

        public PlaylistController(IPlaylistService playlistService, IMusicService musicService)
        {
            _playlistService = playlistService;
            _musicService = musicService;
        }

        [HttpGet("/playlists")]
        public async Task<ActionResult> Index()
        {
            var model = new PlaylistPageDto
            {
                PlayListCategories = await _playlistService.GetPlayListCategories(),
                FeaturedPlayLists = await _playlistService.GetFeaturedPlayLists(),
            };
            return View(model);
        }
        [HttpGet("/playlists/{categoryId}")]
        public async Task<ActionResult> PlaylistsByCategory(long categoryId)
        {
            var category = await _playlistService.GetCategoryBy(categoryId);
            if (category == null) return RedirectToAction("Index");
            ViewBag.categoryName = category.Title;
            var model = await _playlistService.GetPlaylistsByCategory(categoryId);
            return View(model);
        }
        [HttpGet("/playlist/{id}")]
        public async Task<ActionResult> ShowPlayList(long id, long itemId)
        {
            var playlist = await _playlistService.GetPlayListForSiteBy(id);
            if (playlist == null) return NotFound();
            var item = await _musicService.GetMusicForSiteBy(itemId);
            if (item == null)
                return View(new ShowPlaylistWithItemPage { PlayList = playlist });
           await _musicService.AddPlaysMusic(item);
            return View(new ShowPlaylistWithItemPage
            {
                PlayList = playlist,
                Music = item
            });
        }
    }
}
