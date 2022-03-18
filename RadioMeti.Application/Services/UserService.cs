using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.DTOs.Admin.Users;
using RadioMeti.Application.DTOs.Admin.Users.Create;
using RadioMeti.Application.DTOs.Admin.Users.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUser(CreateUserDto create)
    {
        var user = new IdentityUser()
        {
            Email = create.Email,
            UserName = create.UserName,
            EmailConfirmed = true
        };
        return await _userManager.CreateAsync(user, create.Password);
    }

    public async Task<IdentityResult?> DeleteUser(string userName)
    {
        var user=await _userManager.FindByNameAsync(userName);
        if(user==null) return null;
       return await _userManager.DeleteAsync(user);
    }

    public async Task<IdentityResult?> EditUser(EditUserDto edit)
    {
        var user = await _userManager.FindByIdAsync(edit.Id);
        if (user == null) return null;
        user.UserName = edit.UserName;
        user.Email = edit.Email;
       var result= await _userManager.UpdateAsync(user);
       var token= await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _userManager.ConfirmEmailAsync(user,token);
        return result;
    }

    public async Task<FilterUsersDto> FilterUsersListAsync(FilterUsersDto filter)
    {
        var query = _userManager.Users;
        #region state
        switch (filter.FilterUsersState)
        {
            case FilterUsersState.All:
                break;
            case FilterUsersState.EmailConfrimed:
                query = query.Where(p => p.EmailConfirmed).AsQueryable();
                break;
            case FilterUsersState.EmailNotConfrimed:
                query = query.Where(p => !p.EmailConfirmed).AsQueryable();
                break;
            default:
                break;
        }
        #endregion
        #region filter
        if (!string.IsNullOrEmpty(filter.UserName)) query = query.Where(p => p.UserName.Contains(filter.UserName)).AsQueryable();
        if (!string.IsNullOrEmpty(filter.Email)) query = query.Where(p => p.Email.Contains(filter.Email)).AsQueryable();
        #endregion
        #region paging
        var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
        var allUsers = await query.Paging(pager).Select(p => new UsersListDto { Email = p.Email, UserName = p.UserName }).ToListAsync();
        #endregion
        return filter.SetUsers(allUsers).SetPaging(pager);
    }

    public async Task<IdentityUser> GetById(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<IdentityUser> GetByName(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

   
}