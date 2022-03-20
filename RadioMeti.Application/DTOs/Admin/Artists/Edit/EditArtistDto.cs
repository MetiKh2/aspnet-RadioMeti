using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Artists.Edit
{
    public class EditArtistDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string FullName { get; set; }
        public bool IsPopular { get; set; }
        public string? Avatar { get; set; }
        public string? Image { get; set; }
        public long Id { get; set; }
    }
    public enum EditArtistResult
    {
        Success,
        Error,
        ArtistNotfound
    }
}
