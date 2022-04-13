using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Video;
using RadioMeti.Application.DTOs.Admin.Video.Create;
using RadioMeti.Application.DTOs.Admin.Video.Delete;
using RadioMeti.Application.DTOs.Admin.Video.Edit;
using RadioMeti.Application.Extensions;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class VideoController : AdminBaseController
    {
        private readonly IVideoService _videoService;
        private readonly IArtistService _artistService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IMapper _mapper;

        public VideoController(IMapper mapper, ICaptchaValidator captchaValidator, IVideoService videoService, IArtistService artistService)
        {
            _mapper = mapper;
            _captchaValidator = captchaValidator;
            _videoService = videoService;
            _artistService = artistService;
        }
        [HttpGet("/admin/videos")]
        public async Task<IActionResult> Index(FilterVideoDto filter)
        {
            filter.TakeEntity = 3;
            return View(await _videoService.FilterVideo(filter));
        }
        [HttpGet("/admin/createvideo")]
        public async Task<IActionResult> CreateVideo()
        {
            ViewBag.Artists =await _artistService.GetArtists();
            return View();
        }
        [HttpPost("/admin/createvideo"),ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<IActionResult> CreateVideo(CreateVideoDto create, IFormFile? cover, IFormFile? video,List<long> selectedArtists)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            if (create == null)
            {
                TempData[ErrorMessage] = "Video file is heavy";
                return View();
            }
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

                    if (!cover.AddImageToServer(create.Cover, PathExtension.CoverVideoOriginSever, 300, 200, PathExtension.CoverVideoThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                #region Upload Video

                if (video != null)
                {
                    create.VideoFile = video.FileName;

                    if (!video.AddVideoToServer(create.VideoFile, PathExtension.VideoOriginSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _videoService.CreateVideo(create);
                await _videoService.CreateVideoArtists(result.Item2,selectedArtists);
                switch (result.Item1)
                {
                    case CreateVideoResult.Success:
                        TempData[SuccessMessage] = "Video successfull created";
                        break;
                    case CreateVideoResult.Error:
                        TempData[ErrorMessage] = "Something is wrong";
                        break;
                    default:
                        break;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(create);
        }    

        [HttpGet("/admin/editvideo/{id}")]
        public async Task<IActionResult> EditVideo(long id)
        {
            var video = await _videoService.GetVideoBy(id);
            if (video == null) return NotFound(); 
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.VideoArtists = await _videoService.GetVideoArtists(id);
            return View(_mapper.Map<EditVideoDto>(video));
        }
        [HttpPost("/admin/editvideo/{id}"), ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit =409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<IActionResult> EditVideo(EditVideoDto edit,IFormFile? cover,IFormFile? video,List<long> selectedArtists)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.VideoArtists = await _videoService.GetVideoArtists(edit.Id);
            if (edit == null)
            {
                TempData[ErrorMessage] = "Video file is heavy";
                return View();
            }
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

                    if (!cover.AddImageToServer(coverName, PathExtension.CoverVideoOriginSever, 300, 200, PathExtension.CoverVideoThumbSever, edit.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Cover = coverName;
                }
                #endregion
                #region upload video

                if (video != null)
                {
                    var videoName = video.FileName;

                    if (!video.AddVideoToServer(videoName, PathExtension.VideoOriginSever, edit.VideoFile))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.VideoFile = videoName;
                }
                #endregion
                var result = await _videoService.EditVideo(edit);
                if(result==EditVideoResult.Success)
                {
                    await _videoService.DeleteVideoArtists(edit.Id);
                    await _videoService.CreateVideoArtists(edit.Id,selectedArtists);
                }
                switch (result)
                {
                    case EditVideoResult.Success:
                        TempData[SuccessMessage] = "Video successfull edited";
                        break;
                    case EditVideoResult.Error:
                        TempData[ErrorMessage] = "Something is wrong";
                        break;
                    case EditVideoResult.Notfound:
                        TempData[WarningMessage] = "Video notfound";
                        break;
                    default:
                        break;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(edit);
        }

        [HttpGet("Admin/DeleteVideo/{id}")]
        public async Task<IActionResult> DeleteVideo(long id)
        {
            var result = await _videoService.DeleteVideo(id);
            switch (result)
            {
                case DeleteVideoResult.Success:
                    TempData[WarningMessage] = "Video Deleted";
                    break;
                case DeleteVideoResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteVideoResult.Notfound:
                    TempData[ErrorMessage] = "Video Notfound";
                    break;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
