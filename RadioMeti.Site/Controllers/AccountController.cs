using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Controllers;

public class AccountController : SiteBaseController
{
    private readonly IUserService _userService;
    private readonly IMessageSender _messageSender;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    public AccountController(SignInManager<IdentityUser> signInManager, IUserService userService, UserManager<IdentityUser> userManager, IMessageSender messageSender)
    {
        _signInManager = signInManager;
        _userService = userService;
        _userManager = userManager;
        _messageSender = messageSender;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        if (_signInManager.IsSignedIn(User)) return Redirect("/");
        return View();
    }
    #region Signup
    [HttpGet("Signup")]
    public IActionResult Signup()
    {
        if (_signInManager.IsSignedIn(User)) return Redirect("/");
        return View();
    }
    [HttpPost("Signup")]
    public async Task<IActionResult> Signup(SignupUserDto signup)
    {
        if (_signInManager.IsSignedIn(User)) return Redirect("/");
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
                return Redirect("/");
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
        return Redirect("/");
    }
    #endregion
}