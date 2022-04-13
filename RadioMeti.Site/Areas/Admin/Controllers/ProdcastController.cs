using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Dj;
using RadioMeti.Application.DTOs.Admin.Dj.Create;
using RadioMeti.Application.DTOs.Admin.Dj.Delete;
using RadioMeti.Application.DTOs.Admin.Dj.Edit;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Prodcast;
using RadioMeti.Application.DTOs.Admin.Prodcast.Create;
using RadioMeti.Application.DTOs.Admin.Prodcast.Edit;
using RadioMeti.Application.Extensions;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class ProdcastController : AdminBaseController
    {
        private readonly IProdcastService _prodcastService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IMapper _mapper;
        public ProdcastController(IProdcastService prodcastService, ICaptchaValidator captchaValidator, IMapper mapper)
        {
            _prodcastService = prodcastService;
            _captchaValidator = captchaValidator;
            _mapper = mapper;
        }
        #region Dj
        [HttpGet("admin/djs")]
        public async Task<IActionResult> IndexDj(FilterDjDto filter)
        {
            filter.TakeEntity = 3;
            return View(await _prodcastService.FilterDjs(filter));
        }

        [HttpGet("admin/CreateDj")]
        public IActionResult CreateDj()
        {
            return View();
        }
        [HttpPost("admin/CreateDj"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDj(CreateDjDto create, IFormFile? avatar, IFormFile? image)
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
                    create.Image = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);

                    if (!image.AddImageToServer(create.Image, PathExtension.DjImageOriginSever, 300, 200, PathExtension.DjImageThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                if (avatar != null)
                {
                    create.Avatar = Guid.NewGuid().ToString("N") + Path.GetExtension(avatar.FileName);

                    if (!avatar.AddImageToServer(create.Avatar, PathExtension.DjAvatarOriginSever, 300, 200, PathExtension.DjAvatarThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _prodcastService.CreateDj(create);
                switch (result)
                {
                    case CreateDjResult.Success:
                        TempData[SuccessMessage] = "Dj successfully added";
                        break;
                    case CreateDjResult.Error:
                        TempData[ErrorMessage] = "Something is wrong";
                        break;
                }
                return RedirectToAction(nameof(IndexDj));
            }
            return View(create);
        }

        [HttpGet("admin/EditDj/{id}")]
        public async Task<IActionResult> EditDj(long id)
        {
            var dj = await _prodcastService.GetDjBy(id);
            if (dj == null) return NotFound();
            return View(_mapper.Map<EditDjDto>(dj));
        }
        [HttpPost("admin/EditDj/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDj(EditDjDto edit, IFormFile? avatar, IFormFile? image)
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
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);

                    if (!image.AddImageToServer(imageName, PathExtension.DjImageOriginSever, 300, 200, PathExtension.DjImageThumbSever, edit.Image))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Image = imageName;
                }
                if (avatar != null)
                {
                    var avatarName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatar.FileName);

                    if (!avatar.AddImageToServer(avatarName, PathExtension.DjAvatarOriginSever, 300, 200, PathExtension.DjAvatarThumbSever, edit.Avatar))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Avatar = avatarName;
                }

                #endregion
                var result = await _prodcastService.EditDj(edit);
                switch (result)
                {
                    case EditDjResult.Success:
                        TempData[SuccessMessage] = "Dj successfully edited";
                        break;
                    case EditDjResult.Error:
                        TempData[ErrorMessage] = "Something is wrong";
                        break;
                    case EditDjResult.Notfound:
                        TempData[WarningMessage] = "Dj notfound!";
                        break;
                }
                return RedirectToAction(nameof(IndexDj));
            }
            return View(edit);
        }

        [HttpGet("Admin/DeleteDj/{id}")]
        public async Task<IActionResult> DeleteDj(long id)
        {
            var result = await _prodcastService.DeleteDj(id);
            switch (result)
            {
                case DeleteDjResult.Success:
                    TempData[WarningMessage] = "Artist Deleted";
                    break;
                case DeleteDjResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteDjResult.Notfound:
                    TempData[ErrorMessage] = "Dj Notfound";
                    break;
            }
            return RedirectToAction(nameof(IndexDj));
        }

        #endregion
        #region Prodcast
        [HttpGet("admin/prodcasts/{djId}")]
        public async Task<IActionResult> IndexProdcast(long djId,FilterProdcastDto filter)
        {
            ViewBag.dj = await _prodcastService.GetDjBy(djId);
            filter.DjId = djId;
            filter.TakeEntity = 3;
            return View(await _prodcastService.FilterProdcasts(filter)) ;
        }

        [HttpGet("admin/Prodcast/CreateProdcast/{djId}")]
        public async Task<IActionResult> CreateProdcast(long djId)
        {
            if (!await _prodcastService.ExistDj(djId)) return NotFound();
            return View(new CreateProdcastDto {DjId=djId });
        }
        [HttpPost("admin/Prodcast/CreateProdcast/{djId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProdcast(CreateProdcastDto create, IFormFile? cover, IFormFile? audio)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
                #region upload image

                if (cover != null)
                {
                    create.Cover = Guid.NewGuid().ToString("N") + Path.GetExtension(cover.FileName);

                    if (!cover.AddImageToServer(create.Cover, PathExtension.CoverProdcastOriginSever, 300, 200, PathExtension.CoverProdcastThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                #region upload audio

                if (audio != null)
                {
                    create.Audio = audio.FileName;

                    if (!audio.AddAudioToServer(create.Audio, PathExtension.AudioProdcastOriginSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _prodcastService.CreateProdcast(create);
                switch (result.Item1)
                {
                    case CreateProdcastResult.Success:
                        TempData[SuccessMessage] = "Prodcast Successfully Added";
                        break;
                    case CreateProdcastResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case CreateProdcastResult.DjNotfound:
                        TempData[WarningMessage] = "Dj Not Found";
                        return RedirectToAction(nameof(IndexDj));
                    default:
                        break;
                }
                return RedirectToAction(nameof(IndexProdcast), new {djId=result.Item2 });
            }
            return View(create);
        }
       
        [HttpGet("admin/Prodcast/EditProdcast/{id}")]
        public async Task<IActionResult> EditProdcast(long id)
        {
            var prodcast = await _prodcastService.GetProdcastBy(id);
            if (prodcast == null) return NotFound();
            return View(_mapper.Map<EditProdcastDto>(prodcast));
        }
        [HttpPost("admin/Prodcast/EditProdcast/{id}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProdcast(EditProdcastDto edit, IFormFile? cover, IFormFile? audio)
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

                    if (!cover.AddImageToServer(coverName, PathExtension.CoverProdcastOriginSever, 300, 200, PathExtension.CoverProdcastThumbSever, edit.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Cover = coverName;
                }
                #endregion
                #region upload audio

                if (audio != null)
                {
                    var audioName = audio.FileName;

                    if (!audio.AddAudioToServer(audioName, PathExtension.AudioProdcastOriginSever,edit.Audio))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Audio = audioName;
                }
                #endregion
                var result = await _prodcastService.EditProdcast(edit);
                switch (result)
                {
                    case EditProdcastResult.Success:
                        TempData[SuccessMessage] = "Music Successfully Edited";
                        break;
                    case EditProdcastResult.Error:
                        TempData[ErrorMessage] = "Some Thing Is Wrong";
                        break;
                    case EditProdcastResult.ProdcastNotfound:
                        TempData[WarningMessage] = "Prodcast Notfound";
                        return RedirectToAction("IndexDj");
                    default:
                        break;
                }
                return RedirectToAction(nameof(IndexProdcast), new {djId=edit.DjId});
            }
            return View(edit);
        }
        [HttpGet("Admin/DeleteProdcast/{id}")]
        public async Task<IActionResult> DeleteProdcast(long id, long djId)
        {
            var result = await _prodcastService.DeleteProdcast(id);
            switch (result)
            {
                case DeleteMusicResult.Success:
                    TempData[WarningMessage] = "Prodcast Deleted";
                    break;
                case DeleteMusicResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteMusicResult.Notfound:
                    TempData[ErrorMessage] = "Prodcast Notfound";
                    break;
            }
            return RedirectToAction(nameof(IndexProdcast), new { djId= djId});
        }
        #endregion
    }
}
