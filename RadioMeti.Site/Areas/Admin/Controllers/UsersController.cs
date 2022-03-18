using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Users;
using RadioMeti.Application.DTOs.Admin.Users.Create;
using RadioMeti.Application.DTOs.Admin.Users.Edit;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        public UsersController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }
        [HttpGet("Admin/users")]
        public async Task<IActionResult> Index(FilterUsersDto filter)
        {
            filter.TakeEntity=2;
            return View(await _userService.FilterUsersListAsync(filter));
        }

        [HttpGet("Admin/CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            return View();
        }
        [HttpPost("Admin/CreateUser"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserDto create)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
              var result= await _userService.CreateUser(create);
                if (result.Succeeded)
                {
                    TempData[SuccessMessage] = "User Successful Created";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("",item.Description);
                }
            }
            return View(create);
        }
        [HttpGet("Admin/EditUser/{userName}")]
        public async Task<IActionResult> EditUser(string userName)
        {
            var user = await _userService.GetByName(userName);
            if (user == null)
                return NotFound();
            return View(new EditUserDto {Email=user.Email,UserName=user.UserName,Id=user.Id});
        }

        [HttpPost("Admin/EditUser/{userName}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDto edit)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(edit.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(edit);
            }
            if (ModelState.IsValid)
            {
               var result= await _userService.EditUser(edit);
                if (result == null) return NotFound();
                if (result.Succeeded)
                {
                    TempData[SuccessMessage] = "User Successful Updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
                }
            }
            return View(edit);
        }
        [HttpGet("Admin/DeleteUser/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
           var result= await _userService.DeleteUser(userName);
            if(result == null) return NotFound();   
            if (result.Succeeded) TempData[WarningMessage] = "User Succesful Deleted";
            else
                foreach (var item in result.Errors)
                    TempData[ErrorMessage] = item;
            return RedirectToAction("Index");
        }
    }
}
