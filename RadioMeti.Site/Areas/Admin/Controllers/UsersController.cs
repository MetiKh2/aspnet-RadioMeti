using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Users;
using RadioMeti.Application.DTOs.Admin.Users.Create;
using RadioMeti.Application.DTOs.Admin.Users.Edit;
using RadioMeti.Application.DTOs.Enums;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ICaptchaValidator _captchaValidator;
        public UsersController(IUserService userService, ICaptchaValidator captchaValidator, IPermissionService permissionService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _permissionService = permissionService;
        }
        [Authorize(Policy = "INDEXUSERS")]
        [HttpGet("Admin/users")]
        public async Task<IActionResult> Index(FilterUsersDto filter)
        {
            filter.TakeEntity=5;
            return View(await _userService.FilterUsersList(filter));
        }
        [Authorize(Policy = "CREATEUSER")]
        [HttpGet("Admin/CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles = await _permissionService.GetIdentityRoles();
            return View();
        }
        [Authorize(Policy = "CREATEUSER")]
        [HttpPost("Admin/CreateUser"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserDto create,List<string> selectedRoles)
        {
            ViewBag.Roles = await _permissionService.GetIdentityRoles();
            ViewBag.selectedRoles = selectedRoles;
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
              var result= await _userService.CreateUser(create);
                if (result.Item1.Succeeded)
                {
                    await _userService.UserAddRole(result.Item2,selectedRoles);
                    TempData[SuccessMessage] = "User Successful Created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var item in result.Item1.Errors)
                        ModelState.AddModelError("",item.Description);
                }
            }
            return View(create);
        }
        [Authorize(Policy = "EDITUSER")]
        [HttpGet("Admin/EditUser/{userName}")]
        public async Task<IActionResult> EditUser(string userName)
        {
            ViewBag.Roles = await _permissionService.GetIdentityRoles();
            var user = await _userService.GetByName(userName);
            if (user == null)
                return NotFound();
            ViewBag.selectedRoles = await _userService.GetUserRoles(user.Id);
            return View(new EditUserDto {Email=user.Email,UserName=user.UserName,Id=user.Id});
        }
        [Authorize(Policy = "EDITUSER")]
        [HttpPost("Admin/EditUser/{userName}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDto edit,List<string> selectedRoles)
        {
            ViewBag.Roles = await _permissionService.GetIdentityRoles();
            ViewBag.selectedRoles = selectedRoles;
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
                    await _userService.UserDeleteRole(edit.Id);
                    await _userService.UserAddRole(edit.Id,selectedRoles);
                    TempData[SuccessMessage] = "User Successful Updated";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
                }
            }
            return View(edit);
        }
        [Authorize(Policy = "DELETEUSER")]
        [HttpGet("Admin/DeleteUser/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
           var result= await _userService.DeleteUser(userName);
            if(result == null) return NotFound();   
            if (result.Succeeded) TempData[WarningMessage] = "User Succesful Deleted";
            else
                foreach (var item in result.Errors)
                    TempData[ErrorMessage] = item;
            return RedirectToAction(nameof(Index));
        }
    }
}
