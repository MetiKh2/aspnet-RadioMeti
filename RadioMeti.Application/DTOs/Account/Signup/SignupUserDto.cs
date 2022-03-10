using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Account.Signup
{
    public class SignupUserDto
    {
        [Display(Name = "UserName")]
        [MaxLength(200)]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [MaxLength(200)]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
    }
}
