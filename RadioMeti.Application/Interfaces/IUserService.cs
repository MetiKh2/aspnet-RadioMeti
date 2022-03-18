using Microsoft.AspNetCore.Identity;
using RadioMeti.Application.DTOs.Account.Signup;
using System.Security.Claims;

namespace RadioMeti.Application.Interfaces;

public interface IUserService
{
    Task<IdentityResult> SignupUser(SignupUserDto signup);
    Task<IdentityResult> GenerateEmailConfirmationToke(SignupUserDto signup);
    //bool IsSignedInUser(ClaimsPrincipal user);
}