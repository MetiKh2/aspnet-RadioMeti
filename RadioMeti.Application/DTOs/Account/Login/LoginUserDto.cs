using RadioMeti.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Account.Login
{
    public class LoginUserDto:CaptchaDto
    {
        [Display(Name = "UserName")]
        [MaxLength(200)]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
