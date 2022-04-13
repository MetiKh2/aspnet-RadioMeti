using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Event;
using RadioMeti.Application.DTOs.Admin.Event.Create;
using RadioMeti.Application.DTOs.Admin.Event.Delete;
using RadioMeti.Application.DTOs.Admin.Event.Edit;
using RadioMeti.Application.Extensions;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class EventController : AdminBaseController
    {
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        private readonly IArtistService _artistService;

        public EventController(IEventService eventService, IMapper mapper, ICaptchaValidator captchaValidator, IArtistService artistService)
        {
            _eventService = eventService;
            _mapper = mapper;
            _captchaValidator = captchaValidator;
            _artistService = artistService;
        }
        [HttpGet("/admin/events")]
        public async Task<IActionResult> Index(FilterEventDto filter)
        {
            filter.TakeEntity = 3;
            return View(await _eventService.FilterEvent(filter));
        }
        [HttpGet("admin/createEvent")]
        public async Task<IActionResult> CreateEvent()
        {
            ViewBag.Artists = await _artistService.GetArtists();
            return View();
        }
        [HttpPost("admin/createEvent"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(CreateEventDto create, IFormFile? cover,List<long> selectedArtists)
        {
            ViewBag.Artists = await _artistService.GetArtists();
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

                    if (!cover.AddImageToServer(create.Cover, PathExtension.EventCoverOriginSever, 300, 200, PathExtension.EventCoverThumbSever))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(create);
                    }
                }
                #endregion
                var result = await _eventService.CreateEvent(create);
                if (result.Item1 == CreateEventResult.Success) await _eventService.CreateEventArtists(result.Item2,selectedArtists);
                switch (result.Item1)
                {
                    case CreateEventResult.Success:
                        TempData[SuccessMessage] = "Event Successfully Created";
                        break;
                    case CreateEventResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        break;
                    default:
                        break;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(create);
        }
        [HttpGet("admin/editEvent/{id}")]
        public async Task<IActionResult> EditEvent(long id)
        {
            var eventEdited = await _eventService.GetEventBy(id);
            if (eventEdited == null) return NotFound();
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.ArtistsEvent = await _eventService.GetArtistsEvent(id);
            return View(_mapper.Map<EditEventDto>(eventEdited));
        }
        [HttpPost("admin/editEvent/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(EditEventDto edit, IFormFile? cover,List<long> selectedArtists)
        {
            ViewBag.Artists = await _artistService.GetArtists();
            ViewBag.ArtistsEvent = await _eventService.GetArtistsEvent(edit.Id);
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

                    if (!cover.AddImageToServer(coverName, PathExtension.EventCoverOriginSever, 300, 200, PathExtension.EventCoverThumbSever, edit.Cover))
                    {
                        TempData[ErrorMessage] = "Something Is Wrong";
                        return View(edit);
                    }
                    edit.Cover = coverName;
                }
                #endregion
                var result = await _eventService.EditEvent(edit);
                if(result==EditEventResult.Success)
                {
                   await _eventService.DeleteEventArtists(edit.Id);
                   await _eventService.CreateEventArtists(edit.Id,selectedArtists);
                }
                switch (result)
                {
                    case EditEventResult.Success:
                        TempData[SuccessMessage] = "Event Successfully Edited";
                        break;
                    case EditEventResult.Error:
                        TempData[ErrorMessage] = "Something Is Wrong";
                        break;
                    case EditEventResult.Notfound:
                        TempData[WarningMessage] = "Event notfound";
                        break;
                    default:
                        break;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(edit);
        }
        [HttpGet("Admin/DeleteEvent/{id}")]
        public async Task<IActionResult> DeleteEvent(long id)
        {
            var result = await _eventService.DeleteEvent(id);
            switch (result)
            {
                case DeleteEventResult.Success:
                    TempData[WarningMessage] = "Event Deleted";
                    break;
                case DeleteEventResult.Error:
                    TempData[ErrorMessage] = "Something Wrong";
                    break;
                case DeleteEventResult.Notfound:
                    TempData[ErrorMessage] = "Event Notfound";
                    break;
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
