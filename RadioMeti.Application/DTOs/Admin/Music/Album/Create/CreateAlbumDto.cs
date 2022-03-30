using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Music.Album.Create
{
    public class CreateAlbumDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public bool IsSlider { get; set; }
        public string? Cover { get; set; }
    }
    public enum CreateAlbumResult
    {
        Success,
        Error,
        ArtistNotfound,
    }
}
