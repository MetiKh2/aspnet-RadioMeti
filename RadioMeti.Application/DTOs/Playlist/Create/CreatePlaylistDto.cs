
using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Playlist.Create
{
    public class CreatePlaylistDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public string? Cover { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Creator { get; set; }
    }
    public enum CreatePlaylistResult
    {
        Success,
        Error
    }
}
