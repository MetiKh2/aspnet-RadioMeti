using RadioMeti.Application.DTOs.Common;
using RadioMeti.Application.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Roles.Create
{
    public class CreateRoleDto:CaptchaDto
    {
        [MaxLength(200)]
        [Required]
        public string RoleName { get; set; }
        public PermissionsSite PermissionsSite { get; set; }
    }
}
