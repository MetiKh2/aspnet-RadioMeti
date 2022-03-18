using Microsoft.AspNetCore.Identity;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.DTOs.Admin.Users;
using RadioMeti.Application.DTOs.Admin.Users.Create;
using RadioMeti.Application.DTOs.Admin.Users.Edit;
using System.Security.Claims;

namespace RadioMeti.Application.Interfaces;

public interface IUserService
{
    Task<FilterUsersDto> FilterUsersList(FilterUsersDto filter);
    Task<Tuple<IdentityResult, string>> CreateUser(CreateUserDto create);
    Task<IdentityUser> GetById(string id);
    Task<IdentityUser> GetByName(string userName);
    Task<IdentityResult?> EditUser(EditUserDto edit);
    Task<IdentityResult?> DeleteUser(string userName);
    Task UserAddRole(string userId, List<string> selectedRoles);
    Task UserDeleteRole(string userId);
    Task<IList<string>> GetUserRoles(string userId);
}