using RadioMeti.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Playlist.Edit
{
    public class EditPlaylistDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public string? Cover { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Creator { get; set; }
        public long Id { get; set; }
        public bool IsFeatured { get; set; }
    }
    public enum EditPlaylistResult
    {
        Success,
        Error,
        Notfound
    }
}
