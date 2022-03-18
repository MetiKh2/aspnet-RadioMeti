using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;
namespace RadioMeti.Application.DTOs.Admin.Users.Edit
{
    public class EditUserDto:CaptchaDto
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
        public string Id { get; set; }
    }
}
