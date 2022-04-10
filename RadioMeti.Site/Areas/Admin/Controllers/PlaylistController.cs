using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Playlist;
using RadioMeti.Application.DTOs.Playlist.Category;
using RadioMeti.Application.DTOs.Playlist.Category.Create;
using RadioMeti.Application.DTOs.Playlist.Category.Delete;
using RadioMeti.Application.DTOs.Playlist.Category.Edit;
using RadioMeti.Application.DTOs.Playlist.Create;
using RadioMeti.Application.DTOs.Playlist.Delete;
using RadioMeti.Application.DTOs.Playlist.Edit;
using RadioMeti.Application.Extensions;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class PlaylistController : AdminBaseController
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMusicService _musicService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IMapper _mapper;
        public PlaylistController(IPlaylistService playlistService, ICaptchaValidator captchaValidator, IMapper mapper, IMusicService musicService)
        {
            _playlistService = playlistService;
            _captchaValidator = captchaValidator;
            _mapper = mapper;
            _musicService = musicService;
        }
        #region Category
        [HttpGet("admin/playlist/categories")]
        public async Task<IActionResult> IndexCategory(FilterPlaylistCategoryDto filter)
        {
            filter.TakeEntity = 3;
            return View(await _playlistService.FilterPlaylistCategory(filter));
        }
        [HttpGet("admin/playlist/create-category")]
        public  IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost("admin/playlist/create-category"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreatePlaylistCategoryDto create,IFormFile? cover)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
                #region upload images
                if (cover != null)
                {
                    create.Cover = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(create.Cover, PathExtension.CoverPlaylistCategoryOriginSever, 300, 200, PathExtension.CoverPlaylistCategoryThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _playlistService.CreateCategory(create);
                switch (result)
                {
                    case CreatePlaylistCategoryResult.Success:
                        TempData[SuccessMessage] = "Artist Successfully Created";
                        break;
                    case CreatePlaylistCategoryResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        break;
                    default:
                        break;
                }
                return RedirectToAction("IndexCategory");
            }
            return View(create);
        }

        [HttpGet("admin/playlist/edit-category/{id}")]
        public async Task<IActionResult> EditCategory(long id)
        {
            var category = await _playlistService.GetCategoryBy(id);
            if (category == null) return NotFound();
            return View(new EditPlaylistCategoryDto
            {
                Cover = category.Cover,
                IsInBrowse=category.IsInBrowse,
                Title = category.Title,
                Id = category.Id
            });
        }
        [HttpPost("admin/playlist/edit-category/{id}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(EditPlaylistCategoryDto edit, IFormFile? cover)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(edit.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(edit);
            }
            if (ModelState.IsValid)
            {
                #region upload images

                if (cover != null)
                {
                    var coverName = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(coverName, PathExtension.CoverPlaylistCategoryOriginSever, 300, 200, PathExtension.CoverPlaylistCategoryThumbSever, edit.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Cover = coverName;
                }
                #endregion
                 
                var result = await _playlistService.EditCategory(edit);
                switch (result)
                {
                    case EditCategoryResult.Success:
                        TempData[SuccessMessage] = "Music Successfully Edited";
                        break;
                    case EditCategoryResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case EditCategoryResult.CategoryNotfound:
                        TempData[WarningMessage] = "Category Notfound";
                        break;
                    default:
                        break;
                }
                return RedirectToAction(nameof(IndexCategory));
            }
            return View(edit);
        }

        [HttpGet("Admin/playlist/delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(long id, long djId)
        {
            var result = await _playlistService.DeleteCategory(id);
            switch (result)
            {
                case DeleteCategoryResult.Success:
                    TempData[WarningMessage] = "Category Deleted";
                    break;
                case DeleteCategoryResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteCategoryResult.Notfound:
                    TempData[ErrorMessage] = "Category Notfound";
                    break;
            }
            return RedirectToAction(nameof(IndexCategory));
        }
        #endregion
        #region Playlist
        [HttpGet("admin/playlists")]
        public async Task<IActionResult> IndexPlaylist(FilterPlaylistDto filter)
        {
            filter.TakeEntity = 3;
            return View(await _playlistService.filterPlaylistDto(filter));
        }
        [HttpGet("admin/createplaylist")]
        public async Task<IActionResult> CreatePlaylist()
        {
            ViewBag.Musics = await _musicService.GetMusics();
            ViewBag.Categories = await _playlistService.GetPlayListCategories();
            return View();
        }
        [HttpPost("admin/createplaylist"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistDto create, IFormFile? cover,List<long> selectedCategories,List<long> selectedMusics)
        {
            ViewBag.Musics = await _musicService.GetMusics();
            ViewBag.Categories = await _playlistService.GetPlayListCategories();
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
                #region upload images
                if (cover != null)
                {
                    create.Cover = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(create.Cover, PathExtension.CoverPlaylistOriginSever, 300, 200, PathExtension.CoverPlaylistThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _playlistService.CreatePlaylist(create);
                await _playlistService.CreatePlaylistSelectedCategories(result.Item2,selectedCategories);
                await _playlistService.CreatePlaylistMusics(result.Item2,selectedMusics);
                switch (result.Item1)
                {
                    case CreatePlaylistResult.Success:
                        TempData[SuccessMessage] = "Playlist Successfully Created";
                        break;
                    case CreatePlaylistResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        break;
                    default:
                        break;
                }
                return RedirectToAction(nameof(IndexPlaylist));
            }
            return View(create);
        }
        [HttpGet("admin/EditPlaylist/{id}")]
        public async Task<IActionResult> EditPlaylist(long id)
        {
            var playlist = await _playlistService.GetPlayListBy(id);
            if (playlist == null) return NotFound();
            ViewBag.Musics = await _musicService.GetMusics();
            ViewBag.PlayListMusics = await _playlistService.GetPlaylistMusics(id);
            ViewBag.Categories = await _playlistService.GetPlayListCategories();
            ViewBag.PlaylistSelectedCategories = await _playlistService.GetPlaylistSelectedCategories(id);
            return View(new EditPlaylistDto
            {
                Cover = playlist.Cover,
                Creator = playlist.Creator,
                Title = playlist.Title,
                Id = playlist.Id,
            });
        }
        [HttpPost("admin/EditPlaylist/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlaylist(EditPlaylistDto edit, IFormFile? cover, List<long> selectedCategories, List<long> selectedMusics)
        {
            ViewBag.Musics = await _musicService.GetMusics();
            ViewBag.Categories = await _playlistService.GetPlayListCategories();
            if (!await _captchaValidator.IsCaptchaPassedAsync(edit.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(edit);
            }
            if (ModelState.IsValid)
            {
                #region upload images

                if (cover != null)
                {
                    var coverName = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(coverName, PathExtension.CoverPlaylistOriginSever, 300, 200, PathExtension.CoverPlaylistThumbSever, edit.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Cover = coverName;
                }
                #endregion
                var result = await _playlistService.EditPlaylist(edit);
                if(result==EditPlaylistResult.Notfound)
                {
                    TempData[WarningMessage] = "Playlist notfound";
                    return RedirectToAction(nameof(IndexPlaylist));
                }
                await _playlistService.DeletePlaylistMusics(edit.Id);
                await _playlistService.DeletePlaylistSelectedCategories(edit.Id);

                await _playlistService.CreatePlaylistSelectedCategories(edit.Id, selectedCategories);
                await _playlistService.CreatePlaylistMusics(edit.Id, selectedMusics);
                switch (result)
                {
                    case EditPlaylistResult.Success:
                        TempData[SuccessMessage] = "Playlist Successfully Created";
                        break;
                    case EditPlaylistResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        break;
                }
                return RedirectToAction(nameof(IndexPlaylist));
            }
            return View(edit);
        }
        [HttpGet("Admin/DeletePlaylist/{id}")]
        public async Task<IActionResult> DeletePlaylist(long id, long djId)
        {
            var result = await _playlistService.DeletePlaylist(id);
            switch (result)
            {
                case DeletePlaylistResult.Success:
                    TempData[WarningMessage] = "Playlist Deleted";
                    break;
                case DeletePlaylistResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeletePlaylistResult.Notfound:
                    TempData[ErrorMessage] = "Playlist Notfound";
                    break;
            }
            return RedirectToAction(nameof(IndexPlaylist));
        }
        #endregion
    }
}
