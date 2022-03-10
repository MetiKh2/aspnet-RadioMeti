using Microsoft.AspNetCore.Identity;
using RadioMeti.Application.DTOs.Account.Signup;
using RadioMeti.Application.Interfaces;

namespace RadioMeti.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult> GenerateEmailConfirmationToke(SignupUserDto signup)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> SignupUser(SignupUserDto signup)
    {
        return await _userManager.CreateAsync(new IdentityUser {
        Email = signup.Email,
        UserName = signup.UserName,
        EmailConfirmed=false
        }, signup.Password);
    }
}