using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Music.Album.Edit
{
    public class EditAlbumDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public bool IsSlider { get; set; }
        public string? Cover { get; set; }
        public long Id { get; set; }
    }
    public enum EditAlbumResult
    {
        Success,
        Error,
        AlbumNotfound,
    }
}
