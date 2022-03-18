using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;
namespace RadioMeti.Application.DTOs.Admin.Users.Create
{
    public class CreateUserDto:CaptchaDto
    {
        [Display(Name = "UserName")]
        [MaxLength(200)]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [MaxLength(200)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
    }
}
