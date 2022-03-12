using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Account.ForgotPassword;
using RadioMeti.Application.DTOs.Account.Login;
using RadioMeti.Application.DTOs.Account.ResetPassword;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers;

public class AccountController : SiteBaseController
{
    #region ctor
    private readonly IUserService _userService;
    private readonly IMessageSender _messageSender;
    private readonly ICaptchaValidator _captchaValidator;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    public AccountController(SignInManager<IdentityUser> signInManager, IUserService userService, UserManager<IdentityUser> userManager, IMessageSender messageSender, ICaptchaValidator captchaValidator)
    {
        _signInManager = signInManager;
        _userService = userService;
        _userManager = userManager;
        _messageSender = messageSender;
        _captchaValidator = captchaValidator;
    }

    #endregion
    #region Login
    [HttpGet("login")]
    public IActionResult Login(string returnUrl= null)
    {
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index","Home");
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginUserDto { ReturnUrl=returnUrl});
    }
    [HttpPost("login"),ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserDto login)
    {
        ViewBag.ReturnUrl = login.ReturnUrl;
        if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
        {
            TempData[ErrorMessage] = "Captcha is require - Try again";
            return View(login);
        }
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
        if (ModelState.IsValid)
        {
           var result=await _signInManager.PasswordSignInAsync(login.UserName,login.Password,true,true);
            if (result.Succeeded)
            {
                TempData[SuccessMessage] = "You are logged in";
                if (!string.IsNullOrEmpty(login.ReturnUrl) && Url.IsLocalUrl(login.ReturnUrl))
                        return Redirect(login.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                TempData[ErrorMessage] = "Your account has been locked due to 5 failed logins - 15 Minutes";
                return View(login);
            }
            ModelState.AddModelError("", "Informaions are wrong");
        }
        return View(login);
    }
    #endregion
    #region Signup
    [HttpGet("Signup")]
    public IActionResult Signup()
    {
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
        return View();
    }
    [HttpPost("Signup"),ValidateAntiForgeryToken]
    public async Task<IActionResult> Signup(SignupUserDto signup)
    {
        if (!await _captchaValidator.IsCaptchaPassedAsync(signup.Captcha))
        {
            TempData[ErrorMessage] = "Captcha is require - Try again";
            return View(signup);
        }
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
        if (ModelState.IsValid)
        {
            var user = new IdentityUser()
            {
                Email = signup.Email,
                UserName = signup.UserName,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user,signup.Password);
            if (result.Succeeded)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailMessage = Url.Action("ConfirmEmail", "Account", new
                {
                    userName = user.UserName,
                    token = emailConfirmationToken
                }, Request.Scheme);
                await _messageSender.SendEmailAsync(user.Email, "Email Confirmation", emailMessage);
                TempData[InfoMessage] = "Activation email sent";
                return RedirectToAction("Index", "Home");
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
        return View(signup);
    }
    public async Task<IActionResult> ConfirmEmail(string userName, string token)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
        {
            return NotFound();
        }
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded) TempData[SuccessMessage] = "Account activated ";
        else TempData[ErrorMessage] = "Account not activated ";
        return RedirectToAction("Index", "Home");
    }
    #endregion
    #region Logout
    [HttpGet("Logout")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }

    #endregion
    #region Forgot Password
    [HttpGet("Forgot-password")]
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost("Forgot-password"),ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
    {
        if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
        {
            TempData[ErrorMessage] = "Captcha is require - Try again";
            return View(forgot);
        }
        if (ModelState.IsValid)
        {
            TempData[InfoMessage] = "If the email is valid, the email will be sent to you";
            var user = await _userManager.FindByEmailAsync(forgot.Email);
            if (user == null)
                return RedirectToAction("Login","Account");
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordEmail = Url.Action("ResetPassword"
                , "Account", new { email = user.Email, token = resetPasswordToken }, Request.Scheme);
            await _messageSender.SendEmailAsync(user.Email, "Reset Password", resetPasswordEmail);
            return RedirectToAction("Login","Account");
        }
        return View(forgot);
    }
    #endregion
    #region ResetPassword
    [HttpGet("reset-password")]
    public IActionResult ResetPassword(string email, string token)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            return RedirectToAction("Index", "Home");

        var reset = new ResetPasswordDto()
        {
            Email = email,
            Token = token
        };

        return View(reset);
    }

    [HttpPost("reset-password"),ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
    {
        if (!await _captchaValidator.IsCaptchaPassedAsync(reset.Captcha))
        {
            TempData[ErrorMessage] = "Captcha is require - Try again";
            return View(reset);
        }
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null) return RedirectToAction("Login", "Account");
            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.NewPassword);
            if (result.Succeeded)
            {
                TempData[SuccessMessage] = "Your password has changed";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(reset);
    }

    #endregion
}