using RadioMeti.Application.DTOs.Admin.Roles.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Roles.Edit
{
    public class EditRoleDto:CreateRoleDto
    {
        public string Id { get; set; }
    }
}
