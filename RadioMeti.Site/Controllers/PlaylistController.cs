using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Playlist;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet("/playlists")]
        public async Task<ActionResult> Index()
        {
            var model = new PlaylistPageDto
            {
                PlayListCategories=await _playlistService.GetPlayListCategories(),
                FeaturedPlayLists=await _playlistService.GetFeaturedPlayLists(),
            };
            return View(model);
        }
    }
}
