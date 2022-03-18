using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Admin.Roles;
using RadioMeti.Application.DTOs.Admin.Roles.Create;
using RadioMeti.Application.DTOs.Admin.Roles.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.Interfaces;
using System.Security.Claims;

namespace RadioMeti.Application.Services
{
    public class PermissionService: IPermissionService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public PermissionService(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<Tuple<IdentityResult,string>> CreateRole(CreateRoleDto create)
        {
            var role = new IdentityRole { 
            Name=create.RoleName
            };
          var result= await _roleManager.CreateAsync(role);
            return Tuple.Create(result,role.Id);
        }

        public async Task<IdentityResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return null;
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> EditRole(EditRoleDto edit)
        {
            var role = await _roleManager.FindByIdAsync(edit.Id);
            if (role == null) return null;
            role.Name = edit.RoleName;
           return await _roleManager.UpdateAsync(role);
        }

        public async Task<FilterRolesDto> FilterRoles(FilterRolesDto filter)
        {
            var query = _roleManager.Roles;
       
            #region filter
            if (!string.IsNullOrEmpty(filter.RoleName)) query = query.Where(p => p.Name.Contains(filter.RoleName)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allUsers = await query.Paging(pager).Select(p=>p.Name).ToListAsync();
            #endregion
            return filter.SetRoles(allUsers).SetPaging(pager);
        }

        public async Task<List<IdentityRole>> GetIdentityRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetRoleByName(string roleName)
        {
           return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<List<string>> GetRoleClaimsName(string id)
        {
            var role=await _roleManager.FindByIdAsync(id);
            var claims= await _roleManager.GetClaimsAsync(role);
            return claims.Select(p=>p.Type).ToList();
        }

        public async Task RoleAddClaim(string id,List<string> selectedClaims)
        {
            var role=await _roleManager.FindByIdAsync(id);
            if (role != null)   
            foreach (var item in selectedClaims)
               await _roleManager.AddClaimAsync(role,new Claim(item.ToUpper(),item.ToUpper()));
        }

        public async Task RoleDeleteClaim(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var roleClaims =await _roleManager.GetClaimsAsync(role);
                foreach (var claim in roleClaims)
                    await _roleManager.RemoveClaimAsync(role,claim);
            }
        }

       
    }
}
