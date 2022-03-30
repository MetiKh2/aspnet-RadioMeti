using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;
namespace RadioMeti.Application.DTOs.Admin.Music.Single.Create
{
    public class CreateSingleTrackDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Poet { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Lyrics { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Arrangement { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Photographer { get; set; }
        [Display]
        [MaxLength(200)]
        public string? CoverArtist { get; set; }
        public string? Cover { get; set; }
        [Display]
        [MaxLength(200)]
        public string? MusicProducer { get; set; }
        
        [Display]
        [MaxLength(200)]
        public string? Tags { get; set; }
        public bool IsSlider { get; set; }
        public string? Audio { get; set; }

    }
    public enum CreateSingleTrackResult
    {
        Success,
        Error,
        ArtistNotfound,
    }
}
