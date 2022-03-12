
using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Account.ForgotPassword
{
  public class ForgotPasswordDto:CaptchaDto
    {
        [EmailAddress]
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
    }
}
