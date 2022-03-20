using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.DTOs.Account.ForgotPassword;
using RadioMeti.Application.DTOs.Account.Login;
using RadioMeti.Application.DTOs.Account.ResetPassword;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.Interfaces;
using RadioMeti.Site.Controllers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RadioMeti.UnitTest
{
    [TestFixture]
    public class AcoountControllerTests
    {
        private Mock<FakeSignInManager> _signInManager;
        private Mock<FakeUserManager> _userManager;
        private Mock<IMessageSender> _messageSender;
        private Mock<ICaptchaValidator> _captchaVaildator;
        private AccountController _accountController;
        private LoginUserDto _loginUser;
        private SignupUserDto _signupUser;
        private ForgotPasswordDto _forgotPassword;
        private ResetPasswordDto _resetPassword;
        private List<IdentityError> _identityErrors;
        [SetUp]
        public void SetUp()
        {
            _identityErrors = new List<IdentityError>() { new IdentityError() { Description="Error1"} ,
            new IdentityError() { Description="Error2"} };
            _resetPassword = new ResetPasswordDto()
            {
               Token="token",
                Captcha = "Captcha",
                Email = "Email@gmail.com",
                ConfirmNewPassword = "Password",
                NewPassword= "Password",

            };
            _forgotPassword = new ForgotPasswordDto { 
            Captcha ="Captcha",
            Email="Email@gmail.com"
            };
            _signupUser=new SignupUserDto()
            {
                Captcha = "Captcha",
                Password = "Password",
                UserName = "UserName",
                Email="Email@gmail.com"
            };
            _loginUser = new LoginUserDto
            {
                Captcha = "Captcha",
                Password = "Password",
                ReturnUrl = "ReturnUrl",
                UserName = "UserName"
            };
            _messageSender = new Mock<IMessageSender>();
            _captchaVaildator = new Mock<ICaptchaValidator>();
            _userManager = new Mock<FakeUserManager>();
            _signInManager = new Mock<FakeSignInManager>();
            _accountController = new AccountController(_signInManager.Object, _userManager.Object, _messageSender.Object, _captchaVaildator.Object);
        }
        #region Login
        [Test]
        public void LoginGet_UserIsSignedIn_ReturnRedirectHomeIndex()
        {
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var res = _accountController.Login("");
            var redirectToActionResult = (RedirectToActionResult)res;
            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName,"Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        [Test]
        public void LoginGet_WhenCalled_ReturnView()
        {
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

            var res = _accountController.Login("ReturnUrl");
            var viewResult = (ViewResult)res;
            var model =(LoginUserDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<LoginUserDto>());
            Assert.AreEqual(model.ReturnUrl, "ReturnUrl" );
        }
        [Test]
        public async Task LoginPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p =>p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ErrorMessage"] = "Captcha is require - Try again";
             
            _accountController.TempData=tempData;
            var res =await _accountController.Login(_loginUser);
            var viewResult =(ViewResult)res;
            var model = (LoginUserDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<LoginUserDto>());
            Assert.AreEqual(model, _loginUser);
        }
        [Test]
        public async Task LoginPost_CaptchaValidAndUserIsSignedIn_ReturnRedirectHomeIndex()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var res = await _accountController.Login(_loginUser);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        [Test]
        public async Task LoginPost_CaptchaValidAndUserIsNotSignedInAndModelIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _accountController.ModelState.AddModelError("","Error");
           
            var res = await _accountController.Login(_loginUser);
            var viewResult = (ViewResult)res;
            var model = (LoginUserDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<LoginUserDto>());
            Assert.AreEqual(model, _loginUser);
        }
        [Test]
        public async Task LoginPost_CaptchaValidAndUserIsNotSignedInAndModelIsValidAndLoginSuccessAndReturnUrlIsValid_ReturnRedirectReturnUrl()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _signInManager.Setup(p => p.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
            _messageSender.Setup(p => p.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true)).Returns(Task.CompletedTask);
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.IsLocalUrl(_loginUser.ReturnUrl)).Returns(true);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _accountController.Url = urlHelper.Object;
            tempData["SuccessMessage"] = "You are logged in";
            _accountController.TempData = tempData;
            var res = await _accountController.Login(_loginUser);
            var redirectResult = (RedirectResult)res;
            
            Assert.That(res, Is.TypeOf<RedirectResult>());
            Assert.AreEqual(redirectResult.Url, _loginUser.ReturnUrl);
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task LoginPost_CaptchaValidAndUserIsNotSignedInAndModelIsValidAndLoginSuccessAndReturnUrlIsEmpty_ReturnRedirectHomeIndex()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _signInManager.Setup(p => p.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["SuccessMessage"] = "You are logged in";

            _accountController.TempData = tempData;
            _loginUser.ReturnUrl = "";
            var res = await _accountController.Login(_loginUser);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task LoginPost_CaptchaValidAndUserIsNotSignedInAndModelIsValidAndLoginSuccessAndReturnUrlIsNotLocaleUrl_ReturnRedirectHomeIndex()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _signInManager.Setup(p => p.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
            _messageSender.Setup(p => p.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true)).Returns(Task.CompletedTask);
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.IsLocalUrl(_loginUser.ReturnUrl)).Returns(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _accountController.Url = urlHelper.Object;
            tempData["SuccessMessage"] = "You are logged in";
            _accountController.TempData = tempData;
            var res = await _accountController.Login(_loginUser);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task LoginPost_CaptchaValidAndUserIsNotSignedInAndModelIsValidAndLoginNotSuccessAndAccountLocked_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _signInManager.Setup(p => p.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.LockedOut);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            tempData["ErrorMessage"] = "Your account has been locked due to 5 failed logins - 15 Minutes";
            _accountController.TempData = tempData;
            var res = await _accountController.Login(_loginUser);
            var viewResult = (ViewResult)res;
            var model = (LoginUserDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<LoginUserDto>());
            Assert.AreEqual(model, _loginUser);
        }
        #endregion
        #region Signup
        [Test]
        public void SignupGet_UserIsSignedIn_ReturnRedirectHomeIndex()
        {
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var res = _accountController.Signup();
            var redirectToActionResult = (RedirectToActionResult)res;
            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        [Test]
        public void SignupGet_WhenCalled_ReturnView()
        {
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

            var res = _accountController.Signup();
            var viewResult = (ViewResult)res;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.IsNull(viewResult.Model);
        }
        [Test]
        public async Task SignupPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ErrorMessage"] = "Captcha is require - Try again";

            _accountController.TempData = tempData;
            var res = await _accountController.Signup(_signupUser);
            var viewResult = (ViewResult)res;
            var model = (SignupUserDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<SignupUserDto>());
            Assert.AreEqual(model, _signupUser);
        }
        [Test]
        public async Task SignupPost_CaptchaValidAndUserIsSignedIn_ReturnRedirectHomeIndex()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var res = await _accountController.Signup(_signupUser);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        [Test]
        public async Task SignupPost_CaptchaValidAndUserIsNotSignedInAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _accountController.ModelState.AddModelError("", "Error");

            var res = await _accountController.Signup(_signupUser);
            var viewResult = (ViewResult)res;
            var model = (SignupUserDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<SignupUserDto>());
            Assert.AreEqual(model, _signupUser);
        }
        [Test]
        public async Task SignupPost_CaptchaValidAndUserIsNotSignedInAndModelStateIsValidAndSignUpSuccess_ReturnRedirectHomeIndex()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _signInManager.Setup(p => p.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _userManager.Setup(p => p.CreateAsync(It.IsAny<IdentityUser>(),It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _messageSender.Setup(p => p.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true)).Returns(Task.CompletedTask); 
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
           // urlHelper.Setup(x => x.Action(It.IsAny<string>())).Returns("fffdddf");
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["InfoMessage"] = "Activation email sent";
            
            _accountController.TempData = tempData;
            _accountController.Url = Mock.Of<IUrlHelper>();

            var res = await _accountController.Signup(_signupUser);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }
        //Todo Fix a few tests of sign up 
        #endregion
        #region ConfirmEmail
        [Test]
        [TestCase("", "Token")]
        [TestCase("UserName", "")]
        public async Task ConfrimEmail_UserNameOrTokenIsNull_ReturnNotFound(string userName, string token)
        {
            var res = await _accountController.ConfirmEmail(userName, token);
            Assert.That(res, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task ConfrimEmail_UserIsNull_ReturnNotFound()
        {
            _userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(()=>null) ;
            var res = await _accountController.ConfirmEmail("userName", "token");
            Assert.That(res, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public async Task ConfrimEmail_UserIsNotNullAndConfrimSuccess_ReturnRedirectHomeIndex()
        {

            _userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
            _userManager.Setup(p => p.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["SuccessMessage"] = "Account activated ";

            _accountController.TempData = tempData;
            var res = await _accountController.ConfirmEmail("userName", "token");
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task ConfrimEmail_UserIsNotNullAndConfrimNotSuccess_ReturnRedirectHomeIndex()
        {

            _userManager.Setup(p => p.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
            _userManager.Setup(p => p.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ErrorMessage"] = "Account not activated ";

            _accountController.TempData = tempData;
            var res = await _accountController.ConfirmEmail("userName", "token");
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        #endregion

        #region Logout
        [Test]
        public async Task Logout_Success_ReturnRedirectHomeIndex()
        {
            _signInManager.Setup(p => p.SignOutAsync()).Returns(Task.CompletedTask);

            var res =await _accountController.Logout();
            var redirectResult = (RedirectResult)res;
            Assert.That(res, Is.TypeOf<RedirectResult>());
            Assert.AreEqual(redirectResult.Url, "/");
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }

        #endregion

        #region ForgotPassword
        [Test]
        public void ForgotGet_WhenCalled_ReturnView()
        {
            var res = _accountController.ForgotPassword();
            Assert.That(res, Is.TypeOf<ViewResult>());
        }
        [Test]
        public async Task ForgotPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ErrorMessage"] = "Captcha is require - Try again";

            _accountController.TempData = tempData;
            var res = await _accountController.ForgotPassword(_forgotPassword);
            var viewResult = (ViewResult)res;
            var model = (ForgotPasswordDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<ForgotPasswordDto>());
            Assert.AreEqual(model, _forgotPassword);
        }
        [Test]
        public async Task ForgotPost_CaptchaValidAndAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _accountController.ModelState.AddModelError("", "Error");
            
            var res = await _accountController.ForgotPassword(_forgotPassword);
            var viewResult = (ViewResult)res;
            var model = (ForgotPasswordDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<ForgotPasswordDto>());
            Assert.AreEqual(model, _forgotPassword);
        }
        [Test]
        public async Task ForgotPost_CaptchaValidAndAndModelStateIsValidAndUserIsNull_ReturnRedirectLogin()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(()=>null);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["InfoMessage"] = "If the email is valid, the email will be sent to you";

            _accountController.TempData = tempData;

            var res = await _accountController.ForgotPassword(_forgotPassword);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Account");
            Assert.AreEqual(redirectToActionResult.ActionName, "Login");
        }
        [Test]
        public async Task ForgotPost_CaptchaValidAndAndModelStateIsValidAndUserIsNotNull_ReturnRedirectLogin()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            _messageSender.Setup(p => p.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>(),true)).Returns(Task.CompletedTask);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            //urlHelper.Setup(x => x.Action(It.IsAny<string>())).Returns("fffdddf");

            tempData["InfoMessage"] = "If the email is valid, the email will be sent to you";

            _accountController.TempData = tempData;
            _accountController.Url = Mock.Of<IUrlHelper>();
            var res = await _accountController.ForgotPassword(_forgotPassword);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Account");
            Assert.AreEqual(redirectToActionResult.ActionName, "Login");
        }
        #endregion

        #region ResetPassword
        [Test]
        [TestCase("","")]
        [TestCase("","Token")]
        [TestCase("Email","")]
        public void ResetGet_EmailOrTokenIsNullOrEmpty_ReturnRedirectHomeIndex(string email, string token)
        {
            var res = _accountController.ResetPassword(email, token);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Home");
            Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        [Test]
        public void ResetGet_EmailOrTokenIsNotNullOrEmpty_ReturnView()
        {
            var res = _accountController.ResetPassword("email", "token");
            var viewResult= (ViewResult)res;
            var model = (ResetPasswordDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.AreEqual(model.Email, "email");
            Assert.AreEqual(model.Token, "token");
            //Assert.AreEqual(redirectToActionResult.ActionName, "Index");
        }
        [Test]
        public async Task ResetPost_CaptchaIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["ErrorMessage"] = "Captcha is require - Try again";

            _accountController.TempData = tempData;
            var res = await _accountController.ResetPassword(_resetPassword);
            var viewResult = (ViewResult)res;
            var model = (ResetPasswordDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<ResetPasswordDto>());
            Assert.AreEqual(model, _resetPassword);
        }
        [Test]
        public async Task ResetPost_CaptchaValidAndAndModelStateIsNotValid_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _accountController.ModelState.AddModelError("", "Error");

            var res = await _accountController.ResetPassword(_resetPassword);
            var viewResult = (ViewResult)res;
            var model = (ResetPasswordDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<ResetPasswordDto>());
            Assert.AreEqual(model, _resetPassword);

        }
        [Test]
        public async Task ResetPost_CaptchaValidAndAndModelStateIsValidAndUserIsNull_ReturnRedirectLogin()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var res = await _accountController.ResetPassword(_resetPassword);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Account");
            Assert.AreEqual(redirectToActionResult.ActionName, "Login");
        }
        [Test]
        public async Task ResetPost_CaptchaValidAndAndModelStateIsValidAndUserIsNotNullAndResetPasswordSuccess_ReturnRedirectLogin()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
            _userManager.Setup(p => p.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["SuccessMessage"] = "Your password has changed";

            _accountController.TempData = tempData;

            var res = await _accountController.ResetPassword(_resetPassword);
            var redirectToActionResult = (RedirectToActionResult)res;

            Assert.That(res, Is.TypeOf<RedirectToActionResult>());
            Assert.AreEqual(redirectToActionResult.ControllerName, "Account");
            Assert.AreEqual(redirectToActionResult.ActionName, "Login");
            Assert.AreEqual(_accountController.ModelState.ErrorCount, 0);
        }
        [Test]
        public async Task ResetPost_CaptchaValidAndAndModelStateIsValidAndUserIsNotNullAndResetPasswordNotSuccess_ReturnSameView()
        {
            _captchaVaildator.Setup(p => p.IsCaptchaPassedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
            _userManager.Setup(p => p.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(_identityErrors.ToArray()));

            var res = await _accountController.ResetPassword(_resetPassword);
            var viewResult = (ViewResult)res;
            var model = (ResetPasswordDto)viewResult.Model;

            Assert.That(res, Is.TypeOf<ViewResult>());
            Assert.That(viewResult.Model, Is.TypeOf<ResetPasswordDto>());
            Assert.AreEqual(model, _resetPassword);
            Assert.AreEqual(_accountController.ModelState.ErrorCount, _identityErrors.Count);
        }
        #endregion
    }
}
