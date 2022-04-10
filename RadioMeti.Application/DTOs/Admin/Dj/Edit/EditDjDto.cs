using RadioMeti.Application.DTOs.Admin.Dj.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Dj.Edit
{
    public class EditDjDto:CreateDjDto
    {
        public long Id { get; set; }
    }
    public enum EditDjResult
    {
      Success,
      Error,
      Notfound
    }
}
