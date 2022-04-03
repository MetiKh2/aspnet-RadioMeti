using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Artists;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Delete;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.IO;
using System.Threading.Tasks;


namespace RadioMeti.UnitTest.Controllers.Admin.Artist
{
    [TestFixture]
    public class ArtistControllerTests
    {
        private ArtistController _artistController;
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IArtistService> _artistService;
        private Mock<IMapper> _mapper;
        //private Mock<UploadImageExtension> _uploadImageExtension;
        private FilterArtistsDto _filterArtistsDto;
        private CreateArtistDto _createArtistDto;
        private EditArtistDto _editArtistDto;
        private IFormFile _image;
        private IFormFile _avatar;
        [SetUp]
        public void SetUp()
        {
            _editArtistDto=new EditArtistDto()
            {
                Avatar = "Avatar",
                Captcha = "Captcha",
                FullName = "FullName",
                Image = "Image",
                IsPopular = true,
            };
            _avatar = new FormFile(Stream.Null,20,20,"Avatar","Avatar");
            _image = new FormFile(Stream.Null,20,20,"Image","Image");
            _createArtistDto = new CreateArtistDto { 
            Avatar="Avatar",
            Captcha="Captcha",
            FullName="FullName",
            Image="Image",
            IsPopular=true
            };
            _filterArtistsDto= new FilterArtistsDto();
            _mapper = new Mock<IMapper>();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _artistService = new Mock<IArtistService>();
            _artistController = new ArtistController(_captchaVaildator.Object, _artistService.Object, _mapper.Object);

        }
        #region Index
        [Test]
        public async Task Index_WhenCalled_ReturnView()
        {
            var filterArtists = new FilterArtistsDto();
            _artistService.Setup(p => p.FilterArtists(_filterArtistsDto)).ReturnsAsync(filterArtists);
            var result = await _artistController.Index(_filterArtistsDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterArtists);
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public void CreateGet_WhenCalled_ReturnView()
        {
           var result= _artistController.CreateArtist();
            Assert.IsNotNull(result);
            Assert.That(result,Is.TypeOf<ViewResult>());
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.CreateArtist(_createArtistDto,_image,_avatar);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createArtistDto);
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _artistController.ModelState.AddModelError("", "Error");
           

            var result = await _artistController.CreateArtist(_createArtistDto, _image, _avatar);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createArtistDto);
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _artistService.Setup(p => p.CreateArtist(_createArtistDto)).ReturnsAsync(CreateArtistResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.CreateArtist(_createArtistDto, null, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _artistService.Setup(p => p.CreateArtist(_createArtistDto)).ReturnsAsync(CreateArtistResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.CreateArtist(_createArtistDto, null, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditGet_ArtistIsNull_ReturnNotfound()
        {
            var artistId = new Random().Next();
            _artistService.Setup(p => p.GetArtistBy(artistId)).ReturnsAsync(() => null);
            var result = await _artistController.EditArtist(artistId);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditUserGet_ArtistValid_ReturnView()
        {
            var artistId = new Random().Next();
            var artist = new Domain.Entities.Music .Artist{ };
            _artistService.Setup(p => p.GetArtistBy(artistId)).ReturnsAsync(artist);
            _mapper.Setup(p => p.Map<EditArtistDto>(artist)).Returns(_editArtistDto) ;

            var result = await _artistController.EditArtist(artistId);
            var viewResult = (ViewResult)result;
            var model = (EditArtistDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditArtistDto>());
            Assert.That(viewResult.Model, Is.EqualTo(_editArtistDto));

        }
        [Test]
        public async Task EditPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.EditArtist(_editArtistDto, _image, _avatar);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editArtistDto);
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _artistController.ModelState.AddModelError("", "Error");


            var result = await _artistController.EditArtist(_editArtistDto, _image, _avatar);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editArtistDto);
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _artistService.Setup(p => p.EditArtist(_editArtistDto)).ReturnsAsync(EditArtistResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.EditArtist(_editArtistDto, null, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _artistService.Setup(p => p.EditArtist(_editArtistDto)).ReturnsAsync(EditArtistResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.EditArtist(_editArtistDto, null, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_artistController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task Delete_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var artistId = new Random().Next();
            _artistService.Setup(p => p.DeleteArtist(artistId)).ReturnsAsync(DeleteArtistResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.DeleteArtist(artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task Delete_DeleteResultArtistNotfound_ReturnRedirectIndex()
        {
            var artistId = new Random().Next();
            _artistService.Setup(p => p.DeleteArtist(artistId)).ReturnsAsync(DeleteArtistResult.ArtistNotfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.DeleteArtist(artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task Delete_DeleteResultFailed_ReturnRedirectIndex()
        {
            var artistId = new Random().Next();
            _artistService.Setup(p => p.DeleteArtist(artistId)).ReturnsAsync(DeleteArtistResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _artistController.TempData = tempData;

            var result = await _artistController.DeleteArtist(artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        #endregion
    }
}
