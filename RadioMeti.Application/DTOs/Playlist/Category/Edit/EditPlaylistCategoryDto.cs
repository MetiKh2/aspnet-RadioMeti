using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Playlist.Category.Edit
{
    public class EditPlaylistCategoryDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public string? Cover { get; set; }
        public bool IsInBrowse { get; set; }
        public long Id { get; set; }
    }
    public enum EditCategoryResult
    {
        Success,
        Error,
        CategoryNotfound
    }
}
