using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadioMeti.Application.DTOs.Admin.Roles;
using RadioMeti.Application.DTOs.Admin.Roles.Create;
using RadioMeti.Application.DTOs.Admin.Roles.Edit;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Site.Areas.Admin.Controllers
{
    public class PermissionController : AdminBaseController
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ICaptchaValidator _captchaValidator;
        public PermissionController(IUserService userService, ICaptchaValidator captchaValidator, IPermissionService permissionService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _permissionService = permissionService;
        }
        [Authorize(Policy = "INDEXROLES")]
        [HttpGet("Admin/Roles")]
        public async Task<IActionResult> Index(FilterRolesDto filter)
        {
            filter.TakeEntity = 5;
            return View(await _permissionService.FilterRoles(filter));
        }
        [Authorize(Policy = "CREATEROLE")]
        [HttpGet("Admin/CreateRole")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Policy = "CREATEROLE")]
        [HttpPost("Admin/CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto create,List<string> selectedClaims)
        {
            ViewBag.selectedClaims = selectedClaims;
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(create);
            }
            if (ModelState.IsValid)
            {
               var result= await _permissionService.CreateRole(create);
                if (result.Item1.Succeeded)
                {
                    await _permissionService.RoleAddClaim(result.Item2, selectedClaims);
                    TempData[SuccessMessage] = "Role Successfully Created";
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Item1.Errors)
                {
                    ModelState.AddModelError("RoleName",item.Description);
                }
            }
            return View(create);
        }
        [Authorize(Policy = "EDITROLE")]
        [HttpGet("Admin/EditRole/{roleName}")]
        public async Task<IActionResult> EditRole(string roleName)
        {
          var role= await _permissionService.GetRoleByName(roleName);
            if (role == null) return NotFound();
            ViewBag.selectedClaims = await _permissionService.GetRoleClaimsName(role.Id);
            return View(new EditRoleDto {RoleName=roleName,Id=role.Id});
        }
        [Authorize(Policy = "EDITROLE")]
        [HttpPost("Admin/EditRole/{roleName}")]
        public async Task<IActionResult> EditRole(EditRoleDto edit, List<string> selectedClaims)
        {
            ViewBag.selectedClaims = selectedClaims;
            if (!await _captchaValidator.IsCaptchaPassedAsync(edit.Captcha))
            {
                TempData[ErrorMessage] = "Captcha is require - Try again";
                return View(edit);
            }
            if (ModelState.IsValid)
            {
                var result = await _permissionService.EditRole(edit);
                if (result == null) return NotFound();
                if (result.Succeeded)
                {
                    await _permissionService.RoleDeleteClaim(edit.Id);
                    await _permissionService.RoleAddClaim(edit.Id, selectedClaims);
                    TempData[SuccessMessage] = "Role Successfully Edited";
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("RoleName", item.Description);
                }
            }
            return View(edit);
        }
        [Authorize(Policy = "DELETEROLE")]
        [HttpGet("Admin/DeleteRole/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _permissionService.DeleteRole(roleName);
            if (result == null) return NotFound();
            if (result.Succeeded) TempData[WarningMessage] = "Role Succesful Deleted";
            else
                foreach (var item in result.Errors)
                    TempData[ErrorMessage] = item;
            return RedirectToAction("Index");
        }

    }
}
