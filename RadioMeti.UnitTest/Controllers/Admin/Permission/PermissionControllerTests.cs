using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Admin.Roles;
using RadioMeti.Application.DTOs.Admin.Roles.Create;
using RadioMeti.Application.DTOs.Admin.Roles.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.UnitTest.Controllers.Admin.Permission
{
    [TestFixture]
    public class PermissionControllerTests
    {
        private Mock<ICaptchaValidator> _captchaVaildator;
        private Mock<IPermissionService> _permissionService;
        private Mock<IUserService> _userService;
        private PermissionController _permissionController;
        private FilterRolesDto _filterRolesDto;
        private CreateRoleDto _createRoleDto;
        private EditRoleDto _editRoleDto;
        private List<string> _selectedClaims;
        private List<IdentityError> _identityErrors;
        [SetUp]
        public void SetUp()
        {
            _identityErrors = new List<IdentityError>() { new IdentityError() { Description="Error1"} ,
            new IdentityError() { Description="Error2"} };
            _editRoleDto = new EditRoleDto { 
            Captcha="Captcha",
            Id="Id",
            RoleName="Name",
            };
            _createRoleDto = new CreateRoleDto { 
            RoleName="Name",
            Captcha="Captcha"
            };
            _selectedClaims = new List<string>();
            _filterRolesDto = new FilterRolesDto();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _permissionService = new Mock<IPermissionService>();
            _userService = new Mock<IUserService>();
            _permissionController = new PermissionController(_userService.Object, _captchaVaildator.Object, _permissionService.Object);
        }
        #region Index
        [Test]
        public async Task Index_WhenCalled_ReturnView()
        {
            var filterRoles = new FilterRolesDto();
            _permissionService.Setup(p => p.FilterRoles(_filterRolesDto)).ReturnsAsync(filterRoles);
            var result = await _permissionController.Index(_filterRolesDto);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, filterRoles);
        }
        #endregion
        #region Create
        public void CreateRoleGet_WhenCalled_ReturnView()
        {
            var result =  _permissionController.CreateRole();
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }
        [Test]
        public async Task CreateRolePost_CaptchaIsNotValid_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.CreateRole(_createRoleDto, _selectedClaims);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createRoleDto);
        }
        [Test]
        public async Task CreateRolePost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionController.ModelState.AddModelError("", "Error");

            var result = await _permissionController.CreateRole(_createRoleDto, _selectedClaims);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createRoleDto);
        }
        [Test]
        public async Task CreateRolePost_CaptchaIsValidAndModelStateIsValidCreateNotSuccess_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionService.Setup(p => p.CreateRole(_createRoleDto)).ReturnsAsync(new Tuple<IdentityResult, string>(IdentityResult.Failed(_identityErrors.ToArray()), ""));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.CreateRole(_createRoleDto,_selectedClaims);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _createRoleDto);
            Assert.AreEqual(_permissionController.ModelState.ErrorCount, _identityErrors.Count);

        }
        [Test]
        public async Task CreateRolePost_CaptchaIsValidAndModelStateIsValidCreateSuccess_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionService.Setup(p => p.CreateRole(_createRoleDto)).ReturnsAsync(new Tuple<IdentityResult, string>(IdentityResult.Success, "Id"));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.CreateRole(_createRoleDto, _selectedClaims);
            var redirectToActionResult = (RedirectToActionResult)result;
            _permissionService.Verify(p => p.RoleAddClaim("Id", _selectedClaims));
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_permissionController.ModelState.ErrorCount, 0);
        }
        #endregion

        #region Edit
        [Test]
        public async Task EditRoleGet_RoleIsNull_ReturnNotfound()
        {
            _permissionService.Setup(p => p.GetRoleByName("Role")).ReturnsAsync(() => null);
            var result = await _permissionController.EditRole("Role");
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditRoleGet_RoleValid_ReturnView()
        {
            var role = new IdentityRole() { Name="RoleName",Id="Id" };
            _editRoleDto = new EditRoleDto { RoleName = role.Name, Id = role.Id };
            _permissionService.Setup(p => p.GetRoleByName("Role")).ReturnsAsync(role);

            var result = await _permissionController.EditRole("Role");
            _permissionService.Verify(p=>p.GetRoleClaimsName("Id"));
            var viewResult = (ViewResult)result;
            var model = (EditRoleDto)viewResult.Model;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<EditRoleDto>());
            Assert.AreEqual(model.Id, _editRoleDto.Id);
            Assert.AreEqual(model.RoleName, "Role");
        }
        [Test]
        public async Task EditRolePost_CaptchaIsNotValid_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.EditRole(_editRoleDto, _selectedClaims);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editRoleDto);
        }
        [Test]
        public async Task EditRolePost_CaptchaIsValidAndModelStateIsNotValid_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionController.ModelState.AddModelError("", "Error");

            var result = await _permissionController.EditRole(_editRoleDto, _selectedClaims);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editRoleDto);
        }
        [Test]
        public async Task EditRolePost_CaptchaIsValidAndModelStateIsValidEditResultIsNull_ReturnNotfound()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionService.Setup(p => p.EditRole(_editRoleDto)).ReturnsAsync(() => null);

            var result = await _permissionController.EditRole(_editRoleDto, _selectedClaims);
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task EditRolePost_CaptchaIsValidAndModelStateIsValidEditResultFailed_ReturnSameView()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionService.Setup(p => p.EditRole(_editRoleDto)).ReturnsAsync(IdentityResult.Failed(_identityErrors.ToArray()));

            var result = await _permissionController.EditRole(_editRoleDto, _selectedClaims);
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.AreEqual(viewResult.Model, _editRoleDto);
            Assert.AreEqual(_permissionController.ModelState.ErrorCount, _identityErrors.Count);
        }
        [Test]
        public async Task EditRolePost_CaptchaIsValidAndModelStateIsValidEditResultSuccess_ReturnRedirectIndex()
        {
            _selectedClaims.AddRange(new List<string>() { "Claim1", "Claim2", "Claim3" });
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _permissionService.Setup(p => p.EditRole(_editRoleDto)).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.EditRole(_editRoleDto, _selectedClaims);
            var redirectToActionResult = (RedirectToActionResult)result;
            _permissionService.Verify(p => p.RoleAddClaim(_editRoleDto.Id, _selectedClaims));
            _permissionService.Verify(p => p.RoleDeleteClaim(_editRoleDto.Id));
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_permissionController.ModelState.ErrorCount, 0);
        }
        #endregion

        #region Delete
        [Test]
        public async Task DeleteRole_DeleteResultIsNull_ReturnNotfound()
        {
            _permissionService.Setup(p => p.DeleteRole("RoleName")).ReturnsAsync(() => null);

            var result = await _permissionController.DeleteRole("RoleName");
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task DeleteRole_DeleteResultIFailed_ReturnRedirectIndex()
        {
            _permissionService.Setup(p => p.DeleteRole("RoleName")).ReturnsAsync(IdentityResult.Failed(_identityErrors.ToArray()));
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.DeleteRole("RoleName");
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        [Test]
        public async Task DeleteRole_DeleteResultISuccess_ReturnRedirectIndex()
        {
            _permissionService.Setup(p => p.DeleteRole("RoleName")).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _permissionController.TempData = tempData;

            var result = await _permissionController.DeleteRole("RoleName");
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }
        #endregion
    }
}
