using Microsoft.AspNetCore.Identity;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.DTOs.Admin.Users;
using RadioMeti.Application.DTOs.Admin.Users.Create;
using RadioMeti.Application.DTOs.Admin.Users.Edit;
using System.Security.Claims;

namespace RadioMeti.Application.Interfaces;

public interface IUserService
{
    Task<FilterUsersDto> FilterUsersListAsync(FilterUsersDto filter);
    Task<IdentityResult> CreateUser(CreateUserDto create);
    Task<IdentityUser> GetById(string id);
    Task<IdentityUser> GetByName(string userName);
    Task<IdentityResult?> EditUser(EditUserDto edit);
    Task<IdentityResult?> DeleteUser(string userName);
}