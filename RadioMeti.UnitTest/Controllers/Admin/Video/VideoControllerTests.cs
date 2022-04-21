using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Video;
using RadioMeti.Application.DTOs.Admin.Video.Create;
using RadioMeti.Application.DTOs.Admin.Video.Delete;
using RadioMeti.Application.DTOs.Admin.Video.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Video
{
    [TestFixture]
    public class VideoControllerTests
    {
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IVideoService> _videoService;
        private Mock<IArtistService> _artistService;
        private Mock<IMapper> _mapper;
        private VideoController _videoController;
        private FilterVideoDto _filterVideoDto;
        private CreateVideoDto _createVideoDto;
        private EditVideoDto _editVideoDto;
        private IFormFile _cover;
        private IFormFile _video;
        private List<long> _selectedArtists;
        [SetUp]
        public void SetUp()
        {
            _filterVideoDto = new FilterVideoDto();
            _createVideoDto = new CreateVideoDto();
            _editVideoDto = new EditVideoDto() { Id=2};
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _videoService = new Mock<IVideoService>();
            _mapper = new Mock<IMapper>();
            _artistService= new Mock<IArtistService>();
            _videoController = new VideoController(_mapper.Object, _captchaVaildator.Object, _videoService.Object,_artistService.Object);
           
        }

        #region Index
        [Test]
        public async Task Index_WhenCalled_ReturnView()
        {
            var filterVideos= new FilterVideoDto();
            _videoService.Setup(p => p.FilterVideo(_filterVideoDto)).ReturnsAsync(filterVideos);
            var result = await _videoController.Index(_filterVideoDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterVideos);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreateGet_WhenCalled_ReturnView()
        {
            var result = await _videoController.CreateVideo();
            _artistService.Verify(p => p.GetArtists());
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.CreateVideo(_createVideoDto, _cover,_video, _selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createVideoDto);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_DtoNull_ReturnSameView()
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.CreateVideo(null, _cover, _video, _selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.IsNull(viewResult.Model);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoController.ModelState.AddModelError("", "Error");


            var result = await _videoController.CreateVideo(_createVideoDto, _cover, _video, _selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createVideoDto);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidVideoNullCoverNullCreateVideoSuccess_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoService.Setup(p => p.CreateVideo(_createVideoDto)).ReturnsAsync(System.Tuple.Create(CreateVideoResult.Success, id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.CreateVideo(_createVideoDto, _cover, _video, _selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            _videoService.Verify(p => p.CreateVideoArtists(id, _selectedArtists));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidCoverNullCreateVideoFailed_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoService.Setup(p => p.CreateVideo(_createVideoDto)).ReturnsAsync(System.Tuple.Create(CreateVideoResult.Error, id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.CreateVideo(_createVideoDto, _cover, _video, _selectedArtists);
            _videoService.Verify(p=>p.CreateVideoArtists(id,_selectedArtists));
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditGet_VideoIsNull_ReturnNotfound()
        {
            var id = new Random().Next();
            _videoService.Setup(p => p.GetVideoBy(id)).ReturnsAsync(() => null);
            var result = await _videoController.EditVideo(id);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditGet_VideoValid_ReturnView()
        {
            var id = new Random().Next();
            var video = new Domain.Entities.Video.Video { };
            _videoService.Setup(p => p.GetVideoBy(id)).ReturnsAsync(video);
            _mapper.Setup(p => p.Map<EditVideoDto>(video)).Returns(_editVideoDto);

            var result = await _videoController.EditVideo(id);
            _videoService.Verify(p => p.GetVideoArtists(id));
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            var model = (EditVideoDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditVideoDto>());
            Assert.That(viewResult.Model, Is.EqualTo(_editVideoDto));

        }
        [Test]
        public async Task EditPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.EditVideo(_editVideoDto, _cover, _video, _selectedArtists);
            _videoService.Verify(p => p.GetVideoArtists(_editVideoDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editVideoDto);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_DtoNull_ReturnSameView()
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.EditVideo(null, _cover, _video, _selectedArtists);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.IsNull(viewResult.Model);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoController.ModelState.AddModelError("", "Error");


            var result = await _videoController.EditVideo(_editVideoDto, _cover, _video, _selectedArtists);
            _videoService.Verify(p => p.GetVideoArtists(_editVideoDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editVideoDto);
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidVideoNullCoverNullEditVideoSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoService.Setup(p => p.EditVideo(_editVideoDto)).ReturnsAsync(EditVideoResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.EditVideo(_editVideoDto, _cover, _video, _selectedArtists);
            _videoService.Verify(p => p.GetVideoArtists(_editVideoDto.Id));
            _artistService.Verify(p => p.GetArtists());
            _videoService.Verify(p => p.CreateVideoArtists(_editVideoDto.Id, _selectedArtists));
            _videoService.Verify(p => p.DeleteVideoArtists(_editVideoDto.Id));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidVideoNullCoverNullEditVideoNotfound_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoService.Setup(p => p.EditVideo(_editVideoDto)).ReturnsAsync(EditVideoResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.EditVideo(_editVideoDto, _cover, _video, _selectedArtists);
            _videoService.Verify(p => p.GetVideoArtists(_editVideoDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidVideoNullCoverNullAndEditVideoFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _videoService.Setup(p => p.EditVideo(_editVideoDto)).ReturnsAsync(EditVideoResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.EditVideo(_editVideoDto, _cover, _video, _selectedArtists);
            _videoService.Verify(p => p.GetVideoArtists(_editVideoDto.Id));
            _artistService.Verify(p => p.GetArtists());
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_videoController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task Delete_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            _videoService.Setup(p => p.DeleteVideo(id)).ReturnsAsync(DeleteVideoResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.DeleteVideo(id);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task Delete_DeleteResultNotfound_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            _videoService.Setup(p => p.DeleteVideo(id)).ReturnsAsync(DeleteVideoResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.DeleteVideo(id);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task Delete_DeleteResultFailed_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            _videoService.Setup(p => p.DeleteVideo(id)).ReturnsAsync(DeleteVideoResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _videoController.TempData = tempData;

            var result = await _videoController.DeleteVideo(id);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        #endregion
    }
}
