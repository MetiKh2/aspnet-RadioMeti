using Microsoft.AspNetCore.Identity;
using RadioMeti.Application.DTOs.Admin.Roles;
using RadioMeti.Application.DTOs.Admin.Roles.Create;
using RadioMeti.Application.DTOs.Admin.Roles.Edit;

namespace RadioMeti.Application.Interfaces
{
    public interface IPermissionService
    {

        Task<FilterRolesDto> FilterRoles(FilterRolesDto filter);
        Task<Tuple<IdentityResult, string>> CreateRole(CreateRoleDto create);
        Task<IdentityResult> EditRole(EditRoleDto edit);
        Task<IdentityResult> DeleteRole(string roleName);
        Task<IdentityRole> GetRoleByName(string roleName);
        Task RoleAddClaim(string id,List<string> selectedClaims);
        Task RoleDeleteClaim(string id);
        Task<List<string>> GetRoleClaimsName(string id);
        Task<List<IdentityRole>> GetIdentityRoles();
        

    }
}
