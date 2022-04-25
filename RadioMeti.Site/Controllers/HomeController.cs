using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Search;
using RadioMeti.Application.Interfaces;
using System.Diagnostics;

namespace RadioMeti.Site.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly IMusicService _musicService;
        private readonly IProdcastService _prodcastService ;
        private readonly IArtistService _artistService ;
        private readonly IVideoService _videoService ;
        private readonly IPlaylistService _playlistService ;

        public HomeController(IPlaylistService playlistService, IVideoService videoService, IArtistService artistService, IProdcastService prodcastService, IMusicService musicService)
        {
            _playlistService = playlistService;
            _videoService = videoService;
            _artistService = artistService;
            _prodcastService = prodcastService;
            _musicService = musicService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query)) return RedirectToAction("Index");
            ViewBag.Query = query;
            var model = new SearchPageDto { 
            Artists=await _artistService.GetArtists(query,18),
            PlayLists=await _playlistService.GetPlayLists(query,24),
            Djs=await _prodcastService.GetDjs(query,18),
            Musics=await _musicService.GetMusics(query,36),
            Prodcasts=await _prodcastService.GetProdcasts(query,30),
            Videos=await _videoService.GetVideos(query,24),
            Albums=await _musicService.GetAlbums(query,24)
            };
            return View(model);
        }
    }
}