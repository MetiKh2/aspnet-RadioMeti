using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Artists;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Delete;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Application.Extensions;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    
    public class ArtistController : AdminBaseController
    {
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;

        public ArtistController(ICaptchaValidator captchaValidator, IArtistService artistService, IMapper mapper)
        {
            _captchaValidator = captchaValidator;
            _artistService = artistService;
            _mapper = mapper;
        }
        [HttpGet("Admin/Artists")]
        public async Task<IActionResult> Index(FilterArtistsDto filter)
        {
            filter.TakeEntity = 3;
            return View(await _artistService.FilterArtists(filter));
        }
        [HttpGet("Admin/CreateArtist")]
        public IActionResult CreateArtist()
        {
            return View();
        }
        [HttpPost("Admin/CreateArtist"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArtist(CreateArtistDto create,IFormFile? image,IFormFile? avatar)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
                #region upload images
                if (image != null)
                {
                    create.Image= Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);

                    if (!image.AddImageToServer(create.Image, PathExtension.ArtistImageOriginSever, 300, 200, PathExtension.ArtistImageThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                if (avatar != null)
                {
                    create.Avatar = Guid.NewGuid().ToString("N") + Path.GetExtension(avatar.FileName);

                    if (!avatar.AddImageToServer(create.Avatar, PathExtension.ArtistAvatarOriginSever, 300, 200, PathExtension.ArtistAvatarThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _artistService.CreateArtist(create);
                switch (result)
                {
                    case CreateArtistResult.Success:
                        TempData[SuccessMessage] = "Artist Successfully Created";
                        return RedirectToAction("Index");
                    case CreateArtistResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return RedirectToAction("Index");
                    default:
                        break;
                }
            }
            return View(create);
        }

        [HttpGet("Admin/EditArtist/{id}")]
        public async Task<IActionResult> EditArtist(long id)
        {
            var artist = await _artistService.GetArtistBy(id);
            if (artist == null) return NotFound();
            return View(_mapper.Map<EditArtistDto>(artist));
        }
        [HttpPost("Admin/EditArtist/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArtist(EditArtistDto edit, IFormFile? image, IFormFile? avatar)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(edit.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(edit);
            }
            if (ModelState.IsValid)
            {
                #region upload images
                if (image != null)
                {
                    var imageName= Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);

                    if (!image.AddImageToServer(imageName, PathExtension.ArtistImageOriginSever, 300, 200, PathExtension.ArtistImageThumbSever, edit.Image))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return RedirectToAction("Index");
                    }
                    edit.Image = imageName;
                }
                if (avatar != null)
                {
                    var avatarName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatar.FileName);

                    if (!avatar.AddImageToServer(avatarName, PathExtension.ArtistAvatarOriginSever, 300, 200, PathExtension.ArtistAvatarThumbSever, edit.Avatar))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return RedirectToAction("Index");
                    }
                    edit.Avatar = avatarName;
                }
               
                #endregion
                var result = await _artistService.EditArtist(edit);
                switch (result)
                {
                    case EditArtistResult.Success:
                        TempData[SuccessMessage] = "Artist Successfully Edited";
                        return RedirectToAction("Index");
                    case EditArtistResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return RedirectToAction("Index");
                    case EditArtistResult.ArtistNotfound:
                        TempData[WarningMessage] = "Artist Notfounded";
                        return RedirectToAction("Index");
                    default:
                        break;
                }
            }
            return View(edit);
        }

        [HttpGet("Admin/DeleteArtist/{id}")]
        public async Task<IActionResult> DeleteArtist(long id)
        {
            var result = await _artistService.DeleteArtist(id);
            switch (result)
            {
                case DeleteArtistResult.Success:
                    TempData[WarningMessage] = "Artist Deleted";
                    break;
                case DeleteArtistResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteArtistResult.ArtistNotfound:
                    TempData[ErrorMessage] = "Artist Notfound";
                    break;
            }
            return RedirectToAction("Index");
        }
    }

}
