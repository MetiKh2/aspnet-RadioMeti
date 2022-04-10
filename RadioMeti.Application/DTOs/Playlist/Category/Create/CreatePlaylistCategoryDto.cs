using RadioMeti.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Playlist.Category.Create
{
    public class CreatePlaylistCategoryDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public string? Cover { get; set; }
        public bool IsInBrowse { get; set; }
    }
    public enum CreatePlaylistCategoryResult
    {
       Success,
       Error
    }
}
