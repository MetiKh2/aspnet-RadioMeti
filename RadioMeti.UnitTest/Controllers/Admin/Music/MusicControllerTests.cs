using AutoMapper;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Music.Album;
using RadioMeti.Application.DTOs.Admin.Music.Album.Create;
using RadioMeti.Application.DTOs.Admin.Music.Album.Delete;
using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.AlbumMusic.Create;
using RadioMeti.Application.DTOs.Admin.Music.AlbumMusic.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single;
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Music
{
    [TestFixture]
    public class MusicControllerTests
    {
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IArtistService> _artistService;
        private Mock<IMusicService> _musicService;
        private Mock<IMapper> _mapper;
        private MusicController _musicController;
        private FilterMusicsDto _filterMusicsDto;
        private FilterAlbumDto _filterAlbumDto;
        private CreateSingleTrackDto _createSingleTrackDto;
        private CreateAlbumMusicDto _createAlbumMusicDto;
        private CreateAlbumDto _createAlbumDto;
        private EditSingleTrackDto _editSingleTrackDto;
        private EditAlbumMusicDto _editAlbumMusicDto;
        private EditAlbumDto _editAlbumDto;
        private List<long> _artistsId;
        private IFormFile _image;
        private IFormFile _avatar;
        private List<Domain.Entities.Music.Music> _musicList;
        [SetUp]
        public void SetUp()
        {
            _editAlbumMusicDto = new EditAlbumMusicDto()
            {
                AlbumId = new Random().Next()
            };
            _createAlbumMusicDto=   new CreateAlbumMusicDto()
            {
                AlbumId=new Random().Next(),
            };
            _musicList = new List<Domain.Entities.Music.Music>()
            {
                new Domain.Entities.Music.Music{Id=new Random().Next()}
            };
            _editAlbumDto= new EditAlbumDto()
            {
                Id=new Random().Next()
            };
            _createAlbumDto = new CreateAlbumDto();
            _filterAlbumDto = new FilterAlbumDto();
            _editSingleTrackDto=new EditSingleTrackDto()
            {
                Id =new Random().Next()
            };   
            _createSingleTrackDto= new CreateSingleTrackDto();
            _artistsId=new List<long>() { 1,2,3};
            _mapper = new Mock<IMapper>();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _artistService = new Mock<IArtistService>(); 
            _musicService = new Mock<IMusicService>();
            _filterMusicsDto = new FilterMusicsDto() ;
            _musicController = new MusicController(_captchaVaildator.Object,_mapper.Object,_musicService.Object,_artistService.Object);
        }
        #region Single
        #region Index
        [Test]
        public async Task IndexSingle_ArtistNull_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var filterMusics = new FilterMusicsDto();
            _musicService.Setup(p => p.FilterMusics(_filterMusicsDto)).ReturnsAsync(filterMusics);
            _artistService.Setup(p => p.GetArtistBy(artistId)).ReturnsAsync(() => null);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;
            var result = await _musicController.IndexSingleTracks(artistId, _filterMusicsDto);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task IndexSingle_ArtistOk_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var filterMusics = new FilterMusicsDto();
            _musicService.Setup(p => p.FilterMusics(_filterMusicsDto)).ReturnsAsync(filterMusics);
            _artistService.Setup(p => p.GetArtistBy(artistId)).ReturnsAsync(new Domain.Entities.Music.Artist());
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;
            var result = await _musicController.IndexSingleTracks(artistId, _filterMusicsDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterMusics);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreateSingleGet_WhenCalled_ReturnView()
        {
            var result =await _musicController.CreateSingleTrack();
            _artistService.Verify(p=>p.GetArtists());
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateSinglePost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateSingleTrack(_createSingleTrackDto, _image, _avatar,_artistsId);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createSingleTrackDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateSinglePost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicController.ModelState.AddModelError("", "Error");


            var result = await _musicController.CreateSingleTrack(_createSingleTrackDto, _image, _avatar, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createSingleTrackDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreateSinglePost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateSuccess_ReturnRedirectAction()
        {
            long trackId=new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateSingleTrack(_createSingleTrackDto)).ReturnsAsync(Tuple.Create(CreateSingleTrackResult.Success,trackId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateSingleTrack(_createSingleTrackDto, _image, _avatar, _artistsId);
            _artistService.Verify(p => p.GetArtists()); 
            _musicService.Verify(p=>p.CreateArtistsMusic(trackId,_artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateSinglePost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateFailed_ReturnRedirectAction()
        {
            
            long trackId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateSingleTrack(_createSingleTrackDto)).ReturnsAsync(Tuple.Create(CreateSingleTrackResult.Error, trackId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateSingleTrack(_createSingleTrackDto, _image, _avatar, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.CreateArtistsMusic(trackId, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateSinglePost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistNotfound_ReturnRedirectAction()
        {
           
            long trackId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateSingleTrack(_createSingleTrackDto)).ReturnsAsync(Tuple.Create(CreateSingleTrackResult.ArtistNotfound, trackId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateSingleTrack(_createSingleTrackDto, _image, _avatar, _artistsId);
            _artistService.Verify(p => p.GetArtists()); 
            _musicService.Verify(p => p.CreateArtistsMusic(trackId, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature

        #endregion
        #region Edit
        [Test]
        public async Task EditSingleGet_MusicNull_ReturnNotfound()
        {
            var artistId = new Random().Next();
            var musicId = new Random().Next();
            _musicService.Setup(p => p.GetMusicBy(musicId)).ReturnsAsync(()=>null);
            
            var result = await _musicController.EditSingleTrack(musicId,artistId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p=>p.GetArtistsMusic(musicId));
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditSingleGet_MusicOk_ReturnView()
        {
            var artistId = new Random().Next();
            var musicId = new Random().Next();
            var music = new Domain.Entities.Music.Music();
            _musicService.Setup(p => p.GetMusicBy(musicId)).ReturnsAsync(music);
            _mapper.Setup(p => p.Map<EditSingleTrackDto>(music)).Returns(_editSingleTrackDto);
            var result = await _musicController.EditSingleTrack(musicId, artistId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p=>p.GetArtistsMusic(musicId));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editSingleTrackDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditSinglePost_ModelStateIsNotValid_ReturnSameView()
        {
            var artistId = new Random().Next();
            _musicController.ModelState.AddModelError("", "Error");


            var result = await _musicController.EditSingleTrack(_editSingleTrackDto, _image, null, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsMusic(_editSingleTrackDto.Id));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editSingleTrackDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditSinglePost_ModelStateIsValidEditSuccess_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditSingleTrack(_editSingleTrackDto)).ReturnsAsync(EditSingleTrackResult.Success);
            _musicController.TempData = tempData;

            var result = await _musicController.EditSingleTrack(_editSingleTrackDto, _image, null, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsMusic(_editSingleTrackDto.Id));
            _musicService.Verify(p => p.DeleteArtistsMusic(_editSingleTrackDto.Id));
            _musicService.Verify(p => p.CreateArtistsMusic(_editSingleTrackDto.Id, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexSingleTracks");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditSinglePost_ModelStateIsValidEditFailed_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditSingleTrack(_editSingleTrackDto)).ReturnsAsync(EditSingleTrackResult.Error);
            _musicController.TempData = tempData;

            var result = await _musicController.EditSingleTrack(_editSingleTrackDto, _image, null, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsMusic(_editSingleTrackDto.Id));
            _musicService.Verify(p => p.DeleteArtistsMusic(_editSingleTrackDto.Id));
            _musicService.Verify(p => p.CreateArtistsMusic(_editSingleTrackDto.Id, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexSingleTracks");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditSinglePost_ModelStateIsValidEditMusicNotfound_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditSingleTrack(_editSingleTrackDto)).ReturnsAsync(EditSingleTrackResult.MusicNotfound);
            _musicController.TempData = tempData;

            var result = await _musicController.EditSingleTrack(_editSingleTrackDto, _image, null, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsMusic(_editSingleTrackDto.Id));
            _musicService.Verify(p => p.DeleteArtistsMusic(_editSingleTrackDto.Id));
            _musicService.Verify(p => p.CreateArtistsMusic(_editSingleTrackDto.Id, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexSingleTracks");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }

        #endregion
        #region Delete
        [Test]
        public async Task DeleteSingle_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var musicId = new Random().Next();
            var artistId = new Random().Next();
            _musicService.Setup(p => p.DeleteMusic(musicId)).ReturnsAsync(DeleteMusicResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteMusic(musicId, artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexSingleTracks"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
        }
        [Test]
        public async Task DeleteSingle_DeleteResultNotfound_ReturnRedirectIndex()
        {
            var musicId = new Random().Next();
            var artistId = new Random().Next();
            _musicService.Setup(p => p.DeleteMusic(musicId)).ReturnsAsync(DeleteMusicResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteMusic(musicId, artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexSingleTracks"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
        }
        [Test]
        public async Task DeleteSingle_DeleteResultFailed_ReturnRedirectIndex()
        {
            var musicId = new Random().Next();
            var artistId = new Random().Next();
            _musicService.Setup(p => p.DeleteMusic(musicId)).ReturnsAsync(DeleteMusicResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteMusic(musicId, artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexSingleTracks"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
        }
        #endregion
        #endregion

        #region Album
        #region Index
        [Test]
        public async Task IndexAlbum_ArtistNull_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var filterAlbums= new FilterAlbumDto();
            _musicService.Setup(p => p.FilterAlbums(_filterAlbumDto)).ReturnsAsync(filterAlbums);
            _artistService.Setup(p => p.GetArtistBy(artistId)).ReturnsAsync(() => null);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;
            var result = await _musicController.IndexAlbum(artistId, _filterAlbumDto);
            var redirectToActionResult = (RedirectToActionResult)result;
           
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task IndexAlbum_ArtistOk_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var filterAlbums = new FilterAlbumDto();
            _musicService.Setup(p => p.FilterAlbums(_filterAlbumDto)).ReturnsAsync(filterAlbums);
            _artistService.Setup(p => p.GetArtistBy(artistId)).ReturnsAsync(new Domain.Entities.Music.Artist());
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;
            var result = await _musicController.IndexAlbum(artistId, _filterAlbumDto);
            var viewResult = (ViewResult)result;
           
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterAlbums);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreateAlbumGet_WhenCalled_ReturnView()
        {
            var result = await _musicController.CreateAlbum();
            _artistService.Verify(p => p.GetArtists());
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbum(_createAlbumDto, _image, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createAlbumDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicController.ModelState.AddModelError("", "Error");


            var result = await _musicController.CreateAlbum(_createAlbumDto, _image, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createAlbumDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreateAlbumPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateSuccess_ReturnRedirectAction()
        {
            long albumId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateAlbum(_createAlbumDto)).ReturnsAsync(Tuple.Create(CreateAlbumResult.Success, albumId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbum(_createAlbumDto, _image, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.CreateArtistsAlbum(albumId, _artistsId));
           
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateFailed_ReturnRedirectAction()
        {

            long albumId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateAlbum(_createAlbumDto)).ReturnsAsync(Tuple.Create(CreateAlbumResult.Error, albumId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbum(_createAlbumDto, _image, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.CreateArtistsAlbum(albumId, _artistsId));

            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumPost_CaptchaIsValidAndModelStateIsValidImageNullAndAvatarNullCreateArtistNotfound_ReturnRedirectAction()
        {

            long albumId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateAlbum(_createAlbumDto)).ReturnsAsync(Tuple.Create(CreateAlbumResult.ArtistNotfound, albumId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbum(_createAlbumDto, _image, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.CreateArtistsAlbum(albumId, _artistsId));

            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(redirectToActionResult.ControllerName, "Artist");
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature

        #endregion
        #region Edit
        [Test]
        public async Task EditAlbumGet_AlbumNull_ReturnNotfound()
        {
            var artistId = new Random().Next();
            var albumId = new Random().Next();
            _musicService.Setup(p => p.GetAlbumBy(albumId)).ReturnsAsync(() => null);

            var result = await _musicController.EditAlbum(albumId, artistId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsAlbum(albumId));
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditAlbumGet_AlbumOk_ReturnView()
        {
            var artistId = new Random().Next();
            var albumId = new Random().Next();
            var album = new Domain.Entities.Music.Album();
            _musicService.Setup(p => p.GetAlbumBy(albumId)).ReturnsAsync(album);
            _mapper.Setup(p => p.Map<EditAlbumDto>(album)).Returns(_editAlbumDto);
            var result = await _musicController.EditAlbum(albumId, artistId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsAlbum(albumId));

            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editAlbumDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditAlbumPost_ModelStateIsNotValid_ReturnSameView()
        {
            var artistId = new Random().Next();
            _musicController.ModelState.AddModelError("", "Error");


            var result = await _musicController.EditAlbum(_editAlbumDto, _image,artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsAlbum(_editAlbumDto.Id));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editAlbumDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditAlbumPost_ModelStateIsValidEditSuccess_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditAlbum(_editAlbumDto)).ReturnsAsync(EditAlbumResult.Success);
            _musicController.TempData = tempData;

            var result = await _musicController.EditAlbum(_editAlbumDto, _image, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsAlbum(_editAlbumDto.Id));
            _musicService.Verify(p => p.DeleteArtistsAlbum(_editAlbumDto.Id));
            _musicService.Verify(p => p.CreateArtistsAlbum(_editAlbumDto.Id, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbum");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditAlbumPost_ModelStateIsValidEditFailed_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditAlbum(_editAlbumDto)).ReturnsAsync(EditAlbumResult.Error);
            _musicController.TempData = tempData;

            var result = await _musicController.EditAlbum(_editAlbumDto, _image, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsAlbum(_editAlbumDto.Id));
            _musicService.Verify(p => p.DeleteArtistsAlbum(_editAlbumDto.Id));
            _musicService.Verify(p => p.CreateArtistsAlbum(_editAlbumDto.Id, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbum");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditAlbumPost_ModelStateIsValidEditAlbumNotfound_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditAlbum(_editAlbumDto)).ReturnsAsync(EditAlbumResult.AlbumNotfound);
            _musicController.TempData = tempData;

            var result = await _musicController.EditAlbum(_editAlbumDto, _image, artistId, _artistsId);
            _artistService.Verify(p => p.GetArtists());
            _musicService.Verify(p => p.GetArtistsAlbum(_editAlbumDto.Id));
            _musicService.Verify(p => p.DeleteArtistsAlbum(_editAlbumDto.Id));
            _musicService.Verify(p => p.CreateArtistsAlbum(_editAlbumDto.Id, _artistsId));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbum");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Delete
        [Test]
        public async Task DeleteAlbum_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var albumId = new Random().Next();
            var artistId = new Random().Next();
            _musicService.Setup(p => p.DeleteAlbum(albumId)).ReturnsAsync(DeleteAlbumResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteAlbum(albumId, artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexAlbum"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
        }
        [Test]
        public async Task DeleteAlbum_DeleteResultNotfound_ReturnRedirectIndex()
        {
            var albumId = new Random().Next();
            var artistId = new Random().Next();
            _musicService.Setup(p => p.DeleteAlbum(albumId)).ReturnsAsync(DeleteAlbumResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteAlbum(albumId, artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexAlbum"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
        }
        [Test]
        public async Task DeleteAlbum_DeleteResultFailed_ReturnRedirectIndex()
        {
            var albumId = new Random().Next();
            var artistId = new Random().Next();
            _musicService.Setup(p => p.DeleteAlbum(albumId)).ReturnsAsync(DeleteAlbumResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteAlbum(albumId, artistId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexAlbum"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "artistId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { artistId });
        }
        #endregion
        #endregion

        #region AlbumMusic
        #region Index
        [Test]
        public async Task IndexAlbumMusic_WhenCalled_ReturnView()
        {
            var albumId = new Random().Next();
            _musicService.Setup(p => p.GetAlbumMusics(albumId)).ReturnsAsync(_musicList);

            var result = await _musicController.IndexAlbumMusic(albumId);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _musicList);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Create
        [Test]
        public async Task CreateAlbumMusicGet_WhenCalled_ReturnView()
        {
            var albumId=new Random().Next();    
            var result = await _musicController.CreateAlbumMusic(albumId);
            var viewResult=(ViewResult) result;
            var createAlbumMusicDto = (CreateAlbumMusicDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<CreateAlbumMusicDto>());
            Assert.That(createAlbumMusicDto.AlbumId, Is.EqualTo(albumId));
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumMusicPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbumMusic(_createAlbumMusicDto, _image, null);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createAlbumMusicDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumMusicPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicController.ModelState.AddModelError("", "Error");


            var result = await _musicController.CreateAlbumMusic(_createAlbumMusicDto, _image, null);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createAlbumMusicDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task CreateAlbumMusicPost_CaptchaIsValidAndModelStateIsValidImageNullAndAudioNullCreateSuccess_ReturnRedirectAction()
        {
            long albumId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateAlbumMusic(_createAlbumMusicDto)).ReturnsAsync(Tuple.Create(CreateAlbumMusicResult.Success, albumId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbumMusic(_createAlbumMusicDto, _image, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbumMusic");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { _createAlbumMusicDto.AlbumId});
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumMusicPost_CaptchaIsValidAndModelStateIsValidImageNullAndAudioNullCreateFailed_ReturnRedirectAction()
        {

            long albumId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateAlbumMusic(_createAlbumMusicDto)).ReturnsAsync(Tuple.Create(CreateAlbumMusicResult.Error, albumId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbumMusic(_createAlbumMusicDto, _image, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbumMusic");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { _createAlbumMusicDto.AlbumId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task CreateAlbumMusicPost_CaptchaIsValidAndModelStateIsValidImageNullAndAudioNullCreateNotfound_ReturnRedirectAction()
        {

            long albumId = new Random().Next();
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _musicService.Setup(p => p.CreateAlbumMusic(_createAlbumMusicDto)).ReturnsAsync(Tuple.Create(CreateAlbumMusicResult.AlbumNotfound, albumId));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.CreateAlbumMusic(_createAlbumMusicDto, _image, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbumMusic");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { _createAlbumMusicDto.AlbumId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        //Todo Now In All Tests Image And Avatar Is Null Fix It In Feature
        #endregion
        #region Edit
        [Test]
        public async Task EditAlbumMusicGet_AlbumMusicIsNull_ReturnNotfound()
        {
            var musicId = new Random().Next();
            _musicService.Setup(p => p.GetMusicBy(musicId)).ReturnsAsync(() => null);
            var result = await _musicController.EditAlbumMusic(musicId);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditAlbumMusicGet_AlbumMusicIsOk_ReturnView()
        {
            var musicId = new Random().Next();
            var albumMusic = new Domain.Entities.Music.Music{ };
            _musicService.Setup(p => p.GetMusicBy(musicId)).ReturnsAsync(albumMusic);
            _mapper.Setup(p => p.Map<Application.DTOs.Admin.Music.AlbumMusic.Edit.EditAlbumMusicDto>(albumMusic)).Returns(_editAlbumMusicDto);

            var result = await _musicController.EditAlbumMusic(musicId);
            var viewResult = (ViewResult)result;
            var model = (EditAlbumMusicDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditAlbumMusicDto>());
            Assert.That(viewResult.Model, Is.EqualTo(_editAlbumMusicDto));

        }
        [Test]
        public async Task EditAlbumMusicPost_ModelStateIsNotValid_ReturnSameView()
        {
            var artistId = new Random().Next();
            _musicController.ModelState.AddModelError("", "Error");


            var result = await _musicController.EditAlbumMusic(_editAlbumMusicDto, _image, null);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editAlbumMusicDto);
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 1);
        }
        [Test]
        public async Task EditAlbumMusicPost_ModelStateIsValidEditSuccess_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditSingleTrack(_editSingleTrackDto)).ReturnsAsync(EditSingleTrackResult.Success);
            _musicController.TempData = tempData;

            var result = await _musicController.EditAlbumMusic(_editAlbumMusicDto, _image, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbumMusic");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { _editAlbumMusicDto.AlbumId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task EditAlbumMusicPost_ModelStateIsValidEditFailed_ReturnRedirectToAction()
        {
            var artistId = new Random().Next();
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _musicService.Setup(p => p.EditSingleTrack(_editSingleTrackDto)).ReturnsAsync(EditSingleTrackResult.Error);
            _musicController.TempData = tempData;

            var result = await _musicController.EditAlbumMusic(_editAlbumMusicDto, _image, null);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "IndexAlbumMusic");
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { _editAlbumMusicDto.AlbumId });
            Assert.AreEqual(_musicController.ModelState.ErrorCount, 0);
        }
        #endregion
        #region Delete
        [Test]
        public async Task DeleteAlbumMusic_DeleteResultSuccess_ReturnRedirectIndex()
        {
            var albumId = new Random().Next();
            var musicId = new Random().Next();
            _musicService.Setup(p => p.DeleteAlbumMusic(musicId)).ReturnsAsync(DeleteMusicResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteAlbumMusic(musicId, albumId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexAlbumMusic"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { albumId });
        }
        [Test]
        public async Task DeleteAlbumMusic_DeleteResultNotfound_ReturnRedirectIndex()
        {
            var albumId = new Random().Next();
            var musicId = new Random().Next();
            _musicService.Setup(p => p.DeleteAlbumMusic(musicId)).ReturnsAsync(DeleteMusicResult.Notfound);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteAlbumMusic(musicId, albumId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexAlbumMusic"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { albumId });
        }
        [Test]
        public async Task DeleteAlbumMusic_DeleteResultFailed_ReturnRedirectIndex()
        {
            var albumId = new Random().Next();
            var musicId = new Random().Next();
            _musicService.Setup(p => p.DeleteAlbumMusic(musicId)).ReturnsAsync(DeleteMusicResult.Error);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _musicController.TempData = tempData;

            var result = await _musicController.DeleteAlbumMusic(musicId, albumId);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("IndexAlbumMusic"));
            Assert.AreEqual(redirectToActionResult.RouteValues.Keys, new string[] { "albumId" });
            Assert.AreEqual(redirectToActionResult.RouteValues.Values, new object[] { albumId });
        }
        #endregion
        #endregion
    }
}
