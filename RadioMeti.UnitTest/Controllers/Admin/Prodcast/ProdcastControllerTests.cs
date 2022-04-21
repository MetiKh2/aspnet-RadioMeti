using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Dj;
using RadioMeti.Application.DTOs.Admin.Dj.Create;
using RadioMeti.Application.DTOs.Admin.Dj.Delete;
using RadioMeti.Application.DTOs.Admin.Dj.Edit;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Prodcast;
using RadioMeti.Application.DTOs.Admin.Prodcast.Create;
using RadioMeti.Application.DTOs.Admin.Prodcast.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Prodcast
{
    [TestFixture]
    public class ProdcastControllerTests
    {
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IProdcastService> _prodcastService;
        private Mock<IMapper> _mapper;
        private ProdcastController _prodcastController;
        private CreateDjDto _createDjDto;
        private EditDjDto _editDjDto;
        private FilterDjDto _filterDjDto;
        private CreateProdcastDto _createProdcastDto;
        private EditProdcastDto _editProdcastDto;
        private FilterProdcastDto _filterProdcastDto;
        private IFormFile _avatar;
        private IFormFile _image;
        private IFormFile _audio;
        [SetUp]
        public void SetUp()
        {
            _createDjDto = new CreateDjDto();
            _createProdcastDto = new CreateProdcastDto();
            _editProdcastDto = new EditProdcastDto() { DjId=1};
            _editDjDto = new EditDjDto();
            _filterDjDto = new FilterDjDto();
            _filterProdcastDto = new FilterProdcastDto();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _prodcastService = new Mock<IProdcastService>();
            _mapper = new Mock<IMapper>();
            _prodcastController = new ProdcastController(_prodcastService.Object,_captchaVaildator.Object,_mapper.Object);
        }
        #region DJ
        #region Index
        [Test]
        public async Task IndexDj_WhenCalled_ReturnView()
        {
            var filterDjs = new FilterDjDto();
            _prodcastService.Setup(p => p.FilterDjs(_filterDjDto)).ReturnsAsync(filterDjs);
            var result = await _prodcastController.IndexDj(_filterDjDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model,filterDjs);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public void CreateDjGet_WhenCalled_ReturnView()
        {
            var result = _prodcastController.CreateDj();
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateDjPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateDj(_createDjDto, _avatar, _image);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createDjDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateDjPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastController.ModelState.AddModelError("", "Error");


            var result = await _prodcastController.CreateDj(_createDjDto, _avatar, _image);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createDjDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreateDjPost_CaptchaIsValidAndModelStateIsValidImageNullCoverNullCreateDjSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.CreateDj(_createDjDto)).ReturnsAsync(CreateDjResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateDj(_createDjDto, _avatar, _image);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePost_CaptchaIsValidAndModelStateIsValidCoverNullCreateDjFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.CreateDj(_createDjDto)).ReturnsAsync(CreateDjResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateDj(_createDjDto, _avatar, _image);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditDjGet_DjIsNull_ReturnNotfound()
        {
            var id= new Random().Next();
            _prodcastService.Setup(p => p.GetDjBy(id)).ReturnsAsync(() => null);
            var result = await _prodcastController.EditDj(id);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditDjGet_DjValid_ReturnView()
        {
            var id = new Random().Next();
            var dj = new Domain.Entities.Prodcast.Dj{ };
            _prodcastService.Setup(p => p.GetDjBy(id)).ReturnsAsync(dj);
            _mapper.Setup(p => p.Map<EditDjDto>(dj)).Returns(_editDjDto);

            var result = await _prodcastController.EditDj(id);
            var viewResult = (ViewResult)result;
            var model = (EditDjDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditDjDto>());
            Assert.That(viewResult.Model, Is.EqualTo(_editDjDto));

        }
        [Test]
        public async Task EditDjPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditDj(_editDjDto, _avatar,_image);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editDjDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditDjPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastController.ModelState.AddModelError("", "Error");


            var result = await _prodcastController.EditDj(_editDjDto, _avatar, _image);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editDjDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditDjPost_CaptchaIsValidAndModelStateIsValidImageNullCoverNullEditDjSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.EditDj(_editDjDto)).ReturnsAsync(EditDjResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditDj(_editDjDto, _avatar, _image);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditDjPost_CaptchaIsValidAndModelStateIsValidCoverNullEditDjNotfound_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.EditDj(_editDjDto)).ReturnsAsync(EditDjResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditDj(_editDjDto, _avatar, _image);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);

        }
        [Test]
        public async Task EditDjPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditDjFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.EditDj(_editDjDto)).ReturnsAsync(EditDjResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditDj(_editDjDto, _avatar, _image);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);

        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task DeleteDj_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            _prodcastService.Setup(p => p.DeleteDj(id)).ReturnsAsync(DeleteDjResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.DeleteDj(id);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexDj"));
        }
        [Test]
        public async Task DeleteDj_DeleteResultNotfound_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            _prodcastService.Setup(p => p.DeleteDj(id)).ReturnsAsync(DeleteDjResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.DeleteDj(id);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexDj"));
        }
        [Test]
        public async Task DeleteDj_DeleteResultFailed_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            _prodcastService.Setup(p => p.DeleteDj(id)).ReturnsAsync(DeleteDjResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.DeleteDj(id);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexDj"));
        }
        #endregion
        #endregion

        #region Prodcast
        #region Index
        [Test]
        public async Task IndexProdcast_WhenCalled_ReturnView()
        {
            var filterProdcasts = new FilterProdcastDto();
            long djId = new Random().Next();
            _prodcastService.Setup(p => p.FilterProdcasts(_filterProdcastDto)).ReturnsAsync(filterProdcasts);
            var result = await _prodcastController.IndexProdcast(djId, _filterProdcastDto);
            _prodcastService.Verify(p=>p.GetDjBy(djId));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterProdcasts);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreateProdcastGet_DjNull_ReturnView()
        {
            long djId = new Random().Next();
            _prodcastService.Setup(p => p.ExistDj(djId)).ReturnsAsync(false) ;
            var result =await _prodcastController.CreateProdcast(djId);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateProdcastGet_DjOk_ReturnView()
        {
            long djId = new Random().Next();
            _prodcastService.Setup(p => p.ExistDj(djId)).ReturnsAsync(true);
            var result =await _prodcastController.CreateProdcast(djId);
            var viewResult = (ViewResult)result;
            var model=(CreateProdcastDto) viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(model.DjId,djId);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateProdcastPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateProdcast(_createProdcastDto, _avatar, _audio);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createProdcastDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateProdcastPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastController.ModelState.AddModelError("", "Error");


            var result = await _prodcastController.CreateProdcast(_createProdcastDto, _avatar, _audio);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createProdcastDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreateProdcastPost_CaptchaIsValidAndModelStateIsValidAudioNullCoverNullCreateProdcastSuccess_ReturnRedirectAction()
        {
            long id=new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.CreateProdcast(_createProdcastDto)).ReturnsAsync(Tuple.Create(CreateProdcastResult.Success,id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateProdcast(_createProdcastDto, _avatar, _audio);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexProdcast");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new object[] { "djId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { id });
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateProdcastPost_CaptchaIsValidAndModelStateIsValidCoverNullAudioNullCreateProdcastFailed_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.CreateProdcast(_createProdcastDto)).ReturnsAsync(Tuple.Create(CreateProdcastResult.Error, id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateProdcast(_createProdcastDto, _avatar, _audio);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexProdcast");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new object[] { "djId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { id });
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateProdcastPost_CaptchaIsValidAndModelStateIsValidCoverNullAudioNullCreateProdcast_DjNotfound_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.CreateProdcast(_createProdcastDto)).ReturnsAsync(Tuple.Create(CreateProdcastResult.DjNotfound, id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.CreateProdcast(_createProdcastDto, _avatar, _audio);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditProdcastGet_ProdcastIsNull_ReturnNotfound()
        {
            var id = new Random().Next();
            _prodcastService.Setup(p => p.GetProdcastBy(id)).ReturnsAsync(() => null);
            var result = await _prodcastController.EditProdcast(id);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditProdcastGet_ProdcastValid_ReturnView()
        {
            var id = new Random().Next();
            var dj = new Domain.Entities.Prodcast.Prodcast { };
            _prodcastService.Setup(p => p.GetProdcastBy(id)).ReturnsAsync(dj);
            _mapper.Setup(p => p.Map<EditProdcastDto>(dj)).Returns(_editProdcastDto);

            var result = await _prodcastController.EditProdcast(id);
            var viewResult = (ViewResult)result;
            var model = (EditProdcastDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditProdcastDto>());
            Assert.That(viewResult.Model, Is.EqualTo(_editProdcastDto));

        }
        [Test]
        public async Task EditProdcastPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditProdcast(_editProdcastDto, _avatar, _audio);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editProdcastDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditProdcastPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastController.ModelState.AddModelError("", "Error");


            var result = await _prodcastController.EditProdcast(_editProdcastDto, _avatar, _audio);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editProdcastDto);
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditProdcastPost_CaptchaIsValidAndModelStateIsValidAudioNullCoverNullEditProdcastSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.EditProdcast(_editProdcastDto)).ReturnsAsync(EditProdcastResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditProdcast(_editProdcastDto, _avatar, _audio);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexProdcast");
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] {_editProdcastDto.DjId});
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new object[] { "djId" });
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditProdcastPost_CaptchaIsValidAndModelStateIsValidAudioNullCoverNullEditProdcastNotfound_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.EditProdcast(_editProdcastDto)).ReturnsAsync(EditProdcastResult.ProdcastNotfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditProdcast(_editProdcastDto, _avatar, _audio);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexDj");
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);

        }
        [Test]
        public async Task EditProdcastPost_CaptchaIsValidAndModelStateIsValidAudioNullCoverNullEditProdcastFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _prodcastService.Setup(p => p.EditProdcast(_editProdcastDto)).ReturnsAsync(EditProdcastResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.EditProdcast(_editProdcastDto, _avatar, _audio);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexProdcast");
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { _editProdcastDto.DjId });
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new object[] { "djId" });
            Assert.AreEqual(_prodcastController.ModelState.ErrorCount, 0);

        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task DeleteProdcast_DeleteResultSuccess_ReturnRedirectIndex()
        {
            long id = new Random().NextInt64();
            long djId = new Random().NextInt64();
            _prodcastService.Setup(p => p.DeleteProdcast(id)).ReturnsAsync(DeleteMusicResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.DeleteProdcast(id,djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexProdcast"));
            Assert.That(redirectToActionResult.RouteValues.Keys, Is.EqualTo(new object[] {"djId"}));
            Assert.That(redirectToActionResult.RouteValues.Values, Is.EqualTo(new object[] { djId}));
        }
        [Test]
        public async Task DeleteProdcast_DeleteResultNotfound_ReturnRedirectIndex()
        {
            long id = new Random().NextInt64();
            long djId = new Random().NextInt64();
            _prodcastService.Setup(p => p.DeleteProdcast(id)).ReturnsAsync(DeleteMusicResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.DeleteProdcast(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexProdcast"));
            Assert.That(redirectToActionResult.RouteValues.Keys, Is.EqualTo(new object[] { "djId" }));
            Assert.That(redirectToActionResult.RouteValues.Values, Is.EqualTo(new object[] { djId }));
        }
        [Test]
        public async Task DeleteProdcast_DeleteResultFailed_ReturnRedirectIndex()
        {
            long id = new Random().NextInt64();
            long djId = new Random().NextInt64();
            _prodcastService.Setup(p => p.DeleteProdcast(id)).ReturnsAsync(DeleteMusicResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _prodcastController.TempData = tempData;

            var result = await _prodcastController.DeleteProdcast(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexProdcast"));
            Assert.That(redirectToActionResult.RouteValues.Keys, Is.EqualTo(new object[] { "djId" }));
            Assert.That(redirectToActionResult.RouteValues.Values, Is.EqualTo(new object[] { djId }));
        }
        #endregion
        #endregion

    }
}
