

using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Event;
using RadioMeti.Application.DTOs.Admin.Event.Create;
using RadioMeti.Application.DTOs.Admin.Event.Delete;
using RadioMeti.Application.DTOs.Admin.Event.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Event
{
    [TestFixture]
    public class EventControllerTests
    {
        private EventController _eventController;
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IEventService> _eventService;
        private Mock<IArtistService> _artistService;
        private Mock<IMapper> _mapper;
        private FilterEventDto _filterEventDto;
        private CreateEventDto _createEventDto;
        private EditEventDto _editEventDto;
        private IFormFile _cover;
        private List<long> _selectedArtists;
        [SetUp]
        public void SetUp()
        {
            _selectedArtists=new List<long>() { 1,2,3};
            _filterEventDto = new FilterEventDto();
            _createEventDto = new CreateEventDto();
            _editEventDto = new EditEventDto() { Id=1};
            _mapper = new Mock<IMapper>();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _artistService = new Mock<IArtistService>();
            _eventService= new Mock<IEventService>();
            _eventController = new EventController(_eventService.Object,_mapper.Object,_captchaVaildator.Object,_artistService.Object);
        }

        #region Index
        [Test]
        public async Task Index_WhenCalled_ReturnView()
        {
            var filterEvents= new FilterEventDto();
            _eventService.Setup(p => p.FilterEvent(_filterEventDto)).ReturnsAsync(filterEvents);
            var result = await _eventController.Index(_filterEventDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterEvents);
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreateGet_WhenCalled_ReturnView()
        {
            var result =await _eventController.CreateEvent();
            _artistService.Verify(p=>p.GetArtists());
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.CreateEvent(_createEventDto,_cover,_selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createEventDto);
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventController.ModelState.AddModelError("", "Error");


            var result = await _eventController.CreateEvent(_createEventDto,_cover,_selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createEventDto);
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidCoverNullCreateEventSuccess_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventService.Setup(p => p.CreateEvent(_createEventDto)).ReturnsAsync(System.Tuple.Create(CreateEventResult.Success,id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.CreateEvent(_createEventDto,_cover,_selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidCoverNullCreateEventFailed_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventService.Setup(p => p.CreateEvent(_createEventDto)).ReturnsAsync(System.Tuple.Create(CreateEventResult.Error, id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.CreateEvent(_createEventDto, _cover, _selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditGet_EventIsNull_ReturnNotfound()
        {
            var eventId = new Random().Next();
            _artistService.Setup(p => p.GetArtistBy(eventId)).ReturnsAsync(() => null);
            var result = await _eventController.EditEvent(eventId);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditGet_EventValid_ReturnView()
        {
            var eventId = new Random().Next();
            var selectedEvent = new Domain.Entities.Event.Event { };
            _eventService.Setup(p => p.GetEventBy(eventId)).ReturnsAsync(selectedEvent);
            _mapper.Setup(p => p.Map<EditEventDto>(selectedEvent)).Returns(_editEventDto);

            var result = await _eventController.EditEvent(eventId);
            _eventService.Verify(p => p.GetArtistsEvent(eventId));
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            var model = (EditEventDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditEventDto>());
            Assert.That(viewResult.Model, Is.EqualTo(_editEventDto));

        }
        [Test]
        public async Task EditPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.EditEvent(_editEventDto,_cover,_selectedArtists);
            _eventService.Verify(p => p.GetArtistsEvent(_editEventDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editEventDto);
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventController.ModelState.AddModelError("", "Error");


            var result = await _eventController.EditEvent(_editEventDto, _cover, _selectedArtists);
            _eventService.Verify(p => p.GetArtistsEvent(_editEventDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editEventDto);
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidCoverNullEditEventSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventService.Setup(p => p.EditEvent(_editEventDto)).ReturnsAsync(EditEventResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.EditEvent(_editEventDto, _cover, _selectedArtists);
            _eventService.Verify(p => p.GetArtistsEvent(_editEventDto.Id));
            _artistService.Verify(p => p.GetArtists());
            _eventService.Verify(p=>p.CreateEventArtists(_editEventDto.Id,_selectedArtists));
            _eventService.Verify(p=>p.DeleteEventArtists(_editEventDto.Id));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidCoverNullEditEventNotfound_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventService.Setup(p => p.EditEvent(_editEventDto)).ReturnsAsync(EditEventResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.EditEvent(_editEventDto, _cover, _selectedArtists);
            _eventService.Verify(p => p.GetArtistsEvent(_editEventDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditEventFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _eventService.Setup(p => p.EditEvent(_editEventDto)).ReturnsAsync(EditEventResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.EditEvent(_editEventDto, _cover, _selectedArtists);
            _eventService.Verify(p => p.GetArtistsEvent(_editEventDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_eventController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task Delete_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var eventId = new Random().Next();
            _eventService.Setup(p => p.DeleteEvent(eventId)).ReturnsAsync(DeleteEventResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.DeleteEvent(eventId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task Delete_DeleteResultEventNotfound_ReturnRedirectIndex()
        {
            var eventId = new Random().Next();
            _eventService.Setup(p => p.DeleteEvent(eventId)).ReturnsAsync(DeleteEventResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.DeleteEvent(eventId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task Delete_DeleteResultFailed_ReturnRedirectIndex()
        {
            var eventId = new Random().Next();
            _eventService.Setup(p => p.DeleteEvent(eventId)).ReturnsAsync(DeleteEventResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _eventController.TempData = tempData;

            var result = await _eventController.DeleteEvent(eventId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        #endregion
    }
}
