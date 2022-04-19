using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Playlist;
using RadioMeti.Application.DTOs.Playlist.Category;
using RadioMeti.Application.DTOs.Playlist.Category.Create;
using RadioMeti.Application.DTOs.Playlist.Category.Delete;
using RadioMeti.Application.DTOs.Playlist.Category.Edit;
using RadioMeti.Application.DTOs.Playlist.Create;
using RadioMeti.Application.DTOs.Playlist.Delete;
using RadioMeti.Application.DTOs.Playlist.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Playlist
{
    [TestFixture]
    public class PlaylistControllerTests
    {
        private PlaylistController _playlistController;
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IMusicService> _musicService;
        private Mock<IPlaylistService> _playlistService;
        private Mock<IMapper> _mapper;
        private IFormFile? _cover;
        private  FilterPlaylistCategoryDto _filterPlaylistCategoryDto;
        private  CreatePlaylistCategoryDto _createPlaylistCategoryDto;
        private EditPlaylistCategoryDto _editPlaylistCategoryDto;
        private FilterPlaylistDto _filterPlaylistDto;
        private CreatePlaylistDto _createPlaylistDto;
        private EditPlaylistDto _editPlaylistDto;
        List<long> _selectedMusics;
        List<long> _selectedCategories;
        [SetUp]
        public void SetUp()
        {
            _editPlaylistDto = new EditPlaylistDto();
            _selectedCategories= new List<long>() { 4,5,6};
            _selectedMusics= new List<long>() { 1,2,3};
            _createPlaylistDto = new CreatePlaylistDto();
            _filterPlaylistDto = new FilterPlaylistDto();
            _editPlaylistCategoryDto=new EditPlaylistCategoryDto();
            _createPlaylistCategoryDto=new CreatePlaylistCategoryDto();
            _filterPlaylistCategoryDto = new FilterPlaylistCategoryDto();
            _mapper = new Mock<IMapper>();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _musicService = new Mock<IMusicService>();
            _playlistService = new Mock<IPlaylistService>();
            _playlistController = new PlaylistController(_playlistService.Object,_captchaVaildator.Object,  _mapper.Object,_musicService.Object);

        }

        #region Category
        #region Index
        [Test]
        public async Task IndexCategory_WhenCalled_ReturnView()
        {
            var filterCategories = new FilterPlaylistCategoryDto();
            _playlistService.Setup(p => p.FilterPlaylistCategory(_filterPlaylistCategoryDto)).ReturnsAsync(filterCategories);
            var result = await _playlistController.IndexCategory(_filterPlaylistCategoryDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterCategories);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public void CreateCategoryGet_WhenCalled_ReturnView()
        {
            var result = _playlistController.CreateCategory();
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateCategoryPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.CreateCategory(_createPlaylistCategoryDto,_cover);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createPlaylistCategoryDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateCategoryPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistController.ModelState.AddModelError("", "Error");


            var result = await _playlistController.CreateCategory(_createPlaylistCategoryDto,_cover);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createPlaylistCategoryDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreateCategoryPost_CaptchaIsValidAndModelStateIsValidCoverNullAndCreateSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.CreateCategory(_createPlaylistCategoryDto)).ReturnsAsync(CreatePlaylistCategoryResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.CreateCategory(_createPlaylistCategoryDto, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexCategory");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateCategoryPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.CreateCategory(_createPlaylistCategoryDto)).ReturnsAsync(CreatePlaylistCategoryResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.CreateCategory(_createPlaylistCategoryDto, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexCategory");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditCategroyGet_CategoryIsNull_ReturnNotfound()
        {
            var id= new Random().Next();
            _playlistService.Setup(p => p.GetCategoryBy(id)).ReturnsAsync(() => null);
            var result = await _playlistController.EditCategory(id);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditCategroyGet_CategoryValid_ReturnView()
        {
            var id = new Random().Next();
            var category = new Domain.Entities.Music.PlayListCategory{ Id=id,Cover="Cover",IsInBrowse=true,Title="Title"};
            _playlistService.Setup(p => p.GetCategoryBy(id)).ReturnsAsync(category);

            var result = await _playlistController.EditCategory(id);
            var viewResult = (ViewResult)result;
            var model = (EditPlaylistCategoryDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditPlaylistCategoryDto>());
            Assert.That(model.Id, Is.EqualTo(category.Id));
            Assert.That(model.Cover, Is.EqualTo(category.Cover));
            Assert.That(model.IsInBrowse, Is.EqualTo(category.IsInBrowse));
            Assert.That(model.Title, Is.EqualTo(category.Title));

        }
        [Test]
        public async Task EditCategoryPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditCategory(_editPlaylistCategoryDto, _cover);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editPlaylistCategoryDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditCategoryPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistController.ModelState.AddModelError("", "Error");


            var result = await _playlistController.EditCategory(_editPlaylistCategoryDto, _cover);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editPlaylistCategoryDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditCategoryPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditCategorySuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.EditCategory(_editPlaylistCategoryDto)).ReturnsAsync(EditCategoryResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditCategory(_editPlaylistCategoryDto, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexCategory");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditCategoryPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditCategoryFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.EditCategory(_editPlaylistCategoryDto)).ReturnsAsync(EditCategoryResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditCategory(_editPlaylistCategoryDto, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexCategory");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditCategoryPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditCategoryNotfound_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.EditCategory(_editPlaylistCategoryDto)).ReturnsAsync(EditCategoryResult.CategoryNotfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditCategory(_editPlaylistCategoryDto, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexCategory");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task DeleteCategory_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            var djId = new Random().Next();

            _playlistService.Setup(p => p.DeleteCategory(id)).ReturnsAsync(DeleteCategoryResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.DeleteCategory(id,djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexCategory"));
        }
        [Test]
        public async Task DeleteCategory_DeleteResultArtistNotfound_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            var djId = new Random().Next();

            _playlistService.Setup(p => p.DeleteCategory(id)).ReturnsAsync(DeleteCategoryResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.DeleteCategory(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexCategory"));
        }
        [Test]
        public async Task DeleteCategory_DeleteResultFailed_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            var djId = new Random().Next();

            _playlistService.Setup(p => p.DeleteCategory(id)).ReturnsAsync(DeleteCategoryResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.DeleteCategory(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexCategory"));
        }
        #endregion
        #endregion

        #region Playlist
        #region Index
        [Test]
        public async Task IndexPlaylist_WhenCalled_ReturnView()
        {
            var filterPlaylists= new FilterPlaylistDto();
            _playlistService.Setup(p => p.filterPlaylist(_filterPlaylistDto)).ReturnsAsync(filterPlaylists);
            var result = await _playlistController.IndexPlaylist(_filterPlaylistDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterPlaylists);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreatePlaylistGet_WhenCalled_ReturnView()
        {
            var result =await _playlistController.CreatePlaylist();
            _musicService.Verify(p=>p.GetMusics());
            _playlistService.Verify(p=>p.GetPlayListCategories());
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePlaylistPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.CreatePlaylist(_createPlaylistDto, _cover,_selectedCategories,_selectedMusics);
            _musicService.Verify(p => p.GetMusics());
            _playlistService.Verify(p => p.GetPlayListCategories());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createPlaylistDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePlaylistPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistController.ModelState.AddModelError("", "Error");


            var result = await _playlistController.CreatePlaylist(_createPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            _musicService.Verify(p => p.GetMusics());
            _playlistService.Verify(p => p.GetPlayListCategories());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createPlaylistDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreatePlaylistPost_CaptchaIsValidAndModelStateIsValidCoverNullAndCreateSuccess_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p=>p.CreatePlaylist(_createPlaylistDto)).ReturnsAsync(Tuple.Create(CreatePlaylistResult.Success,id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.CreatePlaylist(_createPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            _musicService.Verify(p => p.GetMusics());
            _playlistService.Verify(p => p.GetPlayListCategories());
            _playlistService.Verify(p => p.CreatePlaylistSelectedCategories(id,_selectedCategories));
            _playlistService.Verify(p => p.CreatePlaylistMusics(id,_selectedMusics));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexPlaylist");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreatePlaylistPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistFailed_ReturnRedirectAction()
        {
            long id = new Random().NextInt64();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.CreatePlaylist(_createPlaylistDto)).ReturnsAsync(Tuple.Create(CreatePlaylistResult.Error, id));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.CreatePlaylist(_createPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            _musicService.Verify(p => p.GetMusics());
            _playlistService.Verify(p => p.GetPlayListCategories());
            _playlistService.Verify(p => p.CreatePlaylistSelectedCategories(id, _selectedCategories));
            _playlistService.Verify(p => p.CreatePlaylistMusics(id, _selectedMusics));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexPlaylist");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditPlaylistGet_PlaylistIsNull_ReturnNotfound()
        {
            var id = new Random().Next();
            _playlistService.Setup(p => p.GetPlayListBy(id)).ReturnsAsync(() => null);
            var result = await _playlistController.EditPlaylist(id);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditPlaylistGet_PlaylistValid_ReturnView()
        {
            var id = new Random().Next();
            var playlist = new Domain.Entities.Music.PlayList{ Id = id, Cover = "Cover", Creator = "Creator", Title = "Title" };
            _playlistService.Setup(p => p.GetPlayListBy(id)).ReturnsAsync(playlist);

            var result = await _playlistController.EditPlaylist(id);
            var viewResult = (ViewResult)result;
            var model = (EditPlaylistDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditPlaylistDto>());
            Assert.That(model.Id, Is.EqualTo(playlist.Id));
            Assert.That(model.Cover, Is.EqualTo(playlist.Cover));
            Assert.That(model.Creator, Is.EqualTo(playlist.Creator));
            Assert.That(model.Title, Is.EqualTo(playlist.Title));

        }
        [Test]
        public async Task EditPlaylistPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditPlaylist(_editPlaylistDto, _cover,_selectedCategories,_selectedMusics);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editPlaylistDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPlaylistPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistController.ModelState.AddModelError("", "Error");

            var result = await _playlistController.EditPlaylist(_editPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editPlaylistDto);
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditPlaylistPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditPlaylistSuccess_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.EditPlaylist(_editPlaylistDto)).ReturnsAsync(EditPlaylistResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditPlaylist(_editPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexPlaylist");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPlaylistPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditPlaylistFailed_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.EditPlaylist(_editPlaylistDto)).ReturnsAsync(EditPlaylistResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditPlaylist(_editPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexPlaylist");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditPlaylistPost_CaptchaIsValidAndModelStateIsValidCoverNullAndEditPlaylistNotfound_ReturnRedirectAction()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _playlistService.Setup(p => p.EditPlaylist(_editPlaylistDto)).ReturnsAsync(EditPlaylistResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.EditPlaylist(_editPlaylistDto, _cover, _selectedCategories, _selectedMusics);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexPlaylist");
            Assert.AreEqual(_playlistController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Delete
        [Test]
        public async Task DeletePlayList_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            var djId = new Random().Next();

            _playlistService.Setup(p => p.DeletePlaylist(id)).ReturnsAsync(DeletePlaylistResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.DeletePlaylist(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexPlaylist"));
        }
        [Test]
        public async Task DeletePlayList_DeleteResultArtistNotfound_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            var djId = new Random().Next();

            _playlistService.Setup(p => p.DeletePlaylist(id)).ReturnsAsync(DeletePlaylistResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.DeletePlaylist(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexPlaylist"));
        }
        [Test]
        public async Task DeletePlayList_DeleteResultFailed_ReturnRedirectIndex()
        {
            var id = new Random().Next();
            var djId = new Random().Next();

            _playlistService.Setup(p => p.DeletePlaylist(id)).ReturnsAsync(DeletePlaylistResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _playlistController.TempData = tempData;

            var result = await _playlistController.DeletePlaylist(id, djId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexPlaylist"));
        }
        #endregion
        #endregion
    }
}
