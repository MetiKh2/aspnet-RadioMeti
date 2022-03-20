using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Users;
using RadioMeti.Application.DTOs.Admin.Users.Create;
using RadioMeti.Application.DTOs.Admin.Users.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Users
{
    [TestFixture]
    public class UsersControllerUnitTests
    {
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IPermissionService> _permissionService;
        private Mock<IUserService> _userService;
        private UsersController _usersController;
        private FilterUsersDto _filterUsersDto;
        private CreateUserDto _createUserDto;
        private List<string> _selectedRoles;
        private EditUserDto _editUserDto;
        private List<IdentityError> _identityErrors;
        [SetUp]
        public void SetUp()
        {
            _identityErrors=new List<IdentityError>() { new IdentityError() { Description="Error1"} ,
            new IdentityError() { Description="Error2"} };
            _editUserDto = new EditUserDto {
                Captcha = "Captcha",
                Email = "Email@gmail.com",
                UserName = "UserName",
                Id="Id"
            };
            _selectedRoles = new List<string>();
            _createUserDto = new CreateUserDto()
            {
                Captcha ="Captcha",
                Email ="Email@gmail.com",
                Password ="Password",
                UserName="UserName"
            };
            _filterUsersDto = new FilterUsersDto();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _permissionService = new Mock<IPermissionService>();
            _userService = new Mock<IUserService>();
            _usersController = new UsersController(_userService.Object,_captchaVaildator.Object,_permissionService.Object);
        }

        #region Index
        [Test]
        public async Task Index_WhenCalled_ReturnView()
        {
            var filterUsers=new FilterUsersDto();
            _userService.Setup(p => p.FilterUsersList(_filterUsersDto)).ReturnsAsync(filterUsers);
          var result= await _usersController.Index(_filterUsersDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result,Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model,filterUsers);
        }
        #endregion

        #region Create
        [Test]
        public async Task CreateUserGet_WhenCalled_ReturnView()
        {
            var result = await _usersController.CreateUser();
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }
        [Test]
        public async Task CreateUserPost_CaptchaIsNotValid_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.CreateUser(_createUserDto, _selectedRoles);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createUserDto);
        }
        [Test]
        public async Task CreateUserPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _usersController.ModelState.AddModelError("", "Error");

            var result = await _usersController.CreateUser(_createUserDto, _selectedRoles);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createUserDto);
        }
        [Test]
        public async Task CreateUserPost_CaptchaIsValidAndModelStateIsValidCreateNotSuccess_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userService.Setup(p => p.CreateUser(_createUserDto)).ReturnsAsync(new Tuple<IdentityResult, string>(IdentityResult.Failed(_identityErrors.ToArray()), ""));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.CreateUser(_createUserDto, _selectedRoles);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createUserDto);
            Assert.AreEqual(_usersController.ModelState.ErrorCount, _identityErrors.Count);

        }
        [Test]
        public async Task CreateUserPost_CaptchaIsValidAndModelStateIsValidCreateSuccess_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userService.Setup(p => p.CreateUser(_createUserDto)).ReturnsAsync(new Tuple<IdentityResult, string>(IdentityResult.Success, "Id"));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.CreateUser(_createUserDto, _selectedRoles);
            var redirectToActionResult = (RedirectToActionResult)result;
            _userService.Verify(p => p.UserAddRole("Id", _selectedRoles));
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_usersController.ModelState.ErrorCount,0);
        }
        #endregion
        #region Edit
        [Test]
        public async Task EditUserGet_UserIsNull_ReturnNotfound()
        {
            _userService.Setup(p => p.GetByName("User")).ReturnsAsync(()=>null) ;
            var result = await _usersController.EditUser("User");
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditUserGet_UserValid_ReturnView()
        {
            var user = new IdentityUser() { Email="Email",UserName="UserName",Id="Id"};
           _editUserDto= new EditUserDto { Email = user.Email, UserName = user.UserName, Id = user.Id };
         _userService.Setup(p => p.GetByName("User")).ReturnsAsync(user);
          
            var result = await _usersController.EditUser("User");
            var viewResult = (ViewResult)result;
            var model=(EditUserDto) viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditUserDto>());
            Assert.AreEqual(model.Id, _editUserDto.Id);
            Assert.AreEqual(model.UserName, _editUserDto.UserName);
            Assert.AreEqual(model.Email, _editUserDto.Email);
        }
        [Test]
        public async Task EditUserPost_CaptchaIsNotValid_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.EditUser(_editUserDto, _selectedRoles);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editUserDto);
        }
        [Test]
        public async Task EditUserPost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _usersController.ModelState.AddModelError("", "Error");

            var result = await _usersController.EditUser(_editUserDto, _selectedRoles);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editUserDto);
        }
        [Test]
        public async Task EditUserPost_CaptchaIsValidAndModelStateIsValidEditResultIsNull_ReturnNotfound()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userService.Setup(p => p.EditUser(_editUserDto)).ReturnsAsync(() => null);

            var result = await _usersController.EditUser(_editUserDto, _selectedRoles);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditUserPost_CaptchaIsValidAndModelStateIsValidEditResultFailed_ReturnSameView()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userService.Setup(p => p.EditUser(_editUserDto)).ReturnsAsync(IdentityResult.Failed(_identityErrors.ToArray()));

            var result = await _usersController.EditUser(_editUserDto, _selectedRoles);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editUserDto);
            Assert.AreEqual(_usersController.ModelState.ErrorCount, _identityErrors.Count);
        }
        [Test]
        public async Task EditUserPost_CaptchaIsValidAndModelStateIsValidEditResultSuccess_ReturnRedirectIndex()
        {
            _selectedRoles.AddRange(new List<string>() { "Role1", "Role2", "Role3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userService.Setup(p => p.EditUser(_editUserDto)).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.EditUser(_editUserDto, _selectedRoles);
            var redirectToActionResult = (RedirectToActionResult)result;
            _userService.Verify(p => p.UserAddRole(_editUserDto.Id, _selectedRoles));
            _userService.Verify(p => p.UserDeleteRole(_editUserDto.Id));
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_usersController.ModelState.ErrorCount, 0);
        }
        #endregion

        #region Delete
        [Test]
        public async Task DeleteUser_DeleteResultIsNull_ReturnNotfound()
        {
            _userService.Setup(p => p.DeleteUser("UserName")).ReturnsAsync(() => null);

            var result = await _usersController.DeleteUser("UserName");
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task DeleteUser_DeleteResultIFailed_ReturnRedirectIndex()
        {
            _userService.Setup(p => p.DeleteUser("UserName")).ReturnsAsync(IdentityResult.Failed(_identityErrors.ToArray()));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.DeleteUser("UserName");
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task DeleteUser_DeleteResultISuccess_ReturnRedirectIndex()
        {
            _userService.Setup(p => p.DeleteUser("UserName")).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _usersController.TempData = tempData;

            var result = await _usersController.DeleteUser("UserName");
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        #endregion
    }
}
