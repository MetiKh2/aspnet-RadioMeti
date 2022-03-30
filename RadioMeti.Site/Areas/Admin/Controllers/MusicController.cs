using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Music.Album;
using RadioMeti.Application.DTOs.Admin.Music.Album.Create;
using RadioMeti.Application.DTOs.Admin.Music.Album.Delete;
using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single;
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
using RadioMeti.Application.Extensions;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class MusicController : AdminBaseController
    {
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IMapper _mapper;
        private readonly IMusicService _musicService;
        private readonly IArtistService _artistService;

        public MusicController(ICaptchaValidator captchaValidator, IMapper mapper, IMusicService musicService, IArtistService artistService)
        {
            _captchaValidator = captchaValidator;
            _mapper = mapper;
            _musicService = musicService;
            _artistService = artistService;
        }

        #region Single
        [HttpGet("admin/SingleTracks/{artistId}")]
        public async Task<IActionResult> IndexSingleTracks(long artistId,FilterMusicsDto filter)
        {
            filter.TakeEntity = 4;
            filter.IsSingle = true ;
            filter.ArtistId =artistId;
            var artist=await _artistService.GetArtistBy(artistId);
            if (artist == null)
            {
                TempData[WarningMessage] = "Artist Notfound";
                return RedirectToAction("Index","Artist");
            }
            ViewBag.ArtistName = artist.FullName;
            return View(await _musicService.FilterMusics(filter));
        }
        [HttpGet("admin/Music/CreateSingle")]
        public async Task<IActionResult> CreateSingleTrack()
        {
            ViewBag.Artists = await _artistService.GetArtists();
            return View();
        }
        [HttpPost("admin/Music/CreateSingle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSingleTrack(CreateSingleTrackDto createSingleTrack, IFormFile? cover,IFormFile? audio, List<long> artistsId)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            if (!await _captchaValidator.IsCaptchaPassedAsync(createSingleTrack.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(createSingleTrack);
            }
            if (ModelState.IsValid)
            {
                #region upload image

                if (cover != null)
                {
                    createSingleTrack.Cover = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(createSingleTrack.Cover, PathExtension.CoverSingleTrackOriginSever, 300, 200, PathExtension.CoverSingleTrackThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(createSingleTrack);
                    }
                }
                #endregion
                #region upload audio
                 
                if (audio != null)
                {
                    createSingleTrack.Audio = audio.FileName;

                    if (!audio.AddAudioToServer(createSingleTrack.Audio,PathExtension.AudioSingleTrackOriginSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(createSingleTrack);
                    }
                }
                #endregion
                var result = await _musicService.CreateSingleTrack(createSingleTrack);
                await _musicService.CreateArtistsMusic(result.Item2,artistsId);
                switch (result.Item1)
                {
                    case CreateSingleTrackResult.Success:
                        TempData[SuccessMessage] = "Music Successfully Added";
                        break;
                    case CreateSingleTrackResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case CreateSingleTrackResult.ArtistNotfound:
                        TempData[WarningMessage] = "Artist Not Found";
                        return RedirectToAction("Index", "Artist");
                    default:
                        break;
                }
                return RedirectToAction("Index","Artist");
            }
            return View(createSingleTrack);
        }
        [HttpGet("admin/Music/EditSingle/{id}")]
        public async Task<IActionResult> EditSingleTrack(long id,long artistId)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.ArtistsMusic =await _musicService.GetArtistsMusic(id) ;
            ViewBag.artistId = artistId;
            var music = await _musicService.GetMusicBy(id);
            if (music == null) return NotFound();
            var model = _mapper.Map<EditSingleTrackDto>(music);
            model.Id = id;
            return View(model);
        }
        [HttpPost("admin/Music/EditSingle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSingleTrack(EditSingleTrackDto editSingleTrack, IFormFile? cover, IFormFile? audio, long artistId, List<long> artistsId)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.artistId = artistId;
            ViewBag.ArtistsMusic =await _musicService.GetArtistsMusic(editSingleTrack.Id);
            if (ModelState.IsValid)
            {
                #region upload images

                if (cover != null)
                {
                    var coverName= Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(coverName, PathExtension.CoverSingleTrackOriginSever, 300, 200, PathExtension.CoverSingleTrackThumbSever,editSingleTrack.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(editSingleTrack);
                    }
                    editSingleTrack.Cover =coverName ;
                }
                #endregion
                #region upload audio

                if (audio != null)
                {
                    var audioName= audio.FileName;

                    if (!audio.AddAudioToServer(audioName, PathExtension.AudioSingleTrackOriginSever,editSingleTrack.Audio))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(editSingleTrack);
                    }
                    editSingleTrack.Audio=audioName ;
                }
                #endregion
                var result = await _musicService.EditSingleTrack(editSingleTrack);
                await _musicService.DeleteArtistsMusic(editSingleTrack.Id);
                await _musicService.CreateArtistsMusic(editSingleTrack.Id,artistsId);
                switch (result)
                {
                    case EditSingleTrackResult.Success:
                        TempData[SuccessMessage] = "Music Successfully Edited";
                        break;
                    case EditSingleTrackResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case EditSingleTrackResult.MusicNotfound:
                        return RedirectToAction(nameof(IndexSingleTracks), new { artistId = artistId });
                    default:
                        break;
                }
                return RedirectToAction(nameof(IndexSingleTracks), new { artistId = artistId });
            }
            return View(editSingleTrack);
        }

        [HttpGet("Admin/DeleteMusic/{id}")]
        public async Task<IActionResult> DeleteMusic(long id,long artistId)
        {
            var result = await _musicService.DeleteMusic(id);
            switch (result)
            {
                case DeleteMusicResult.Success:
                    TempData[WarningMessage] = "Music Deleted";
                    break;
                case DeleteMusicResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteMusicResult.Notfound:
                    TempData[ErrorMessage] = "Music Notfound";
                    break;
            }
            return RedirectToAction(nameof(IndexSingleTracks),new {artistId= artistId});
        }
        #endregion

        #region Album
        [HttpGet("admin/Albums/{artistId}")]
        public async Task<IActionResult> IndexAlbum(long artistId, FilterAlbumDto filter)
        {
            filter.TakeEntity = 4;
            filter.ArtistId = artistId;
            var artist = await _artistService.GetArtistBy(artistId);
            if (artist == null)
            {
                TempData[WarningMessage] = "Artist Notfound";
                return RedirectToAction("Index", "Artist");
            }
            ViewBag.ArtistName = artist.FullName;
            return View(await _musicService.FilterAlbums(filter));
        }
        [HttpGet("admin/Music/CreateAlbum")]
        public async Task<IActionResult> CreateAlbum()
        {
            ViewBag.Artists = await _artistService.GetArtists();
            return View();
        }
        [HttpPost("admin/Music/CreateAlbum")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAlbum(CreateAlbumDto createAlbum, IFormFile? cover, List<long> artistsId)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            if (!await _captchaValidator.IsCaptchaPassedAsync(createAlbum.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(createAlbum);
            }
            if (ModelState.IsValid)
            {
                #region upload image

                if (cover != null)
                {
                    createAlbum.Cover = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(createAlbum.Cover, PathExtension.CoverAlbumOriginSever, 300, 200, PathExtension.CoverAlbumThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(createAlbum);
                    }
                }
                #endregion
                 
                var result = await _musicService.CreateAlbum(createAlbum);
                await _musicService.CreateArtistsAlbum(result.Item2, artistsId);
                switch (result.Item1)
                {
                    case CreateAlbumResult.Success:
                        TempData[SuccessMessage] = "Album Successfully Added";
                        break;
                    case CreateAlbumResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case CreateAlbumResult.ArtistNotfound:
                        TempData[WarningMessage] = "Artist Not Found";
                        break;
                }
                return RedirectToAction("Index", "Artist");
            }
            return View(createAlbum);
        }
        [HttpGet("admin/Music/EditAlbum/{id}")]
        public async Task<IActionResult> EditAlbum(long id, long artistId)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.ArtistsAlbum = await _musicService.GetArtistsAlbum(id);
            ViewBag.artistId = artistId;
            var album = await _musicService.GetAlbumBy(id);
            if (album == null) return NotFound();
            var model = _mapper.Map<EditAlbumDto>(album);
            return View(model);
        }
        [HttpPost("admin/Music/EditAlbum/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAlbum(EditAlbumDto editAlbum, IFormFile? cover, long artistId, List<long> artistsId)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.artistId = artistId;
            ViewBag.ArtistsAlbum= await _musicService.GetArtistsAlbum(editAlbum.Id);
            if (ModelState.IsValid)
            {
                #region upload images

                if (cover != null)
                {
                    var coverName = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(coverName, PathExtension.CoverAlbumOriginSever, 300, 200, PathExtension.CoverAlbumThumbSever, editAlbum.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(editAlbum);
                    }
                    editAlbum.Cover = coverName;
                }
                #endregion
                 
                var result = await _musicService.EditAlbum(editAlbum);
                await _musicService.DeleteArtistsAlbum(editAlbum.Id);
                await _musicService.CreateArtistsAlbum(editAlbum.Id, artistsId);
                switch (result)
                {
                    case EditAlbumResult.Success:
                        TempData[SuccessMessage] = "Album Successfully Edited";
                        break;
                    case EditAlbumResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case EditAlbumResult.AlbumNotfound:
                        return RedirectToAction(nameof(IndexAlbum), new { artistId = artistId });
                    default:
                        break;
                }
                return RedirectToAction(nameof(IndexAlbum), new { artistId = artistId });
            }
            return View(editAlbum);
        }

        [HttpGet("Admin/DeleteAlbum/{id}")]
        public async Task<IActionResult> DeleteAlbum(long id, long artistId)
        {
            var result = await _musicService.DeleteAlbum(id);
            switch (result)
            {
                case DeleteAlbumResult.Success:
                    TempData[WarningMessage] = "Album Deleted";
                    break;
                case DeleteAlbumResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteAlbumResult.Notfound:
                    TempData[ErrorMessage] = "Music Notfound";
                    break;
            }
            return RedirectToAction(nameof(IndexAlbum), new { artistId = artistId });
        }
        #endregion
    }
}
