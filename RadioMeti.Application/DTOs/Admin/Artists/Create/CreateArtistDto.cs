

using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Artists.Create
{
    public class CreateArtistDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string FullName { get; set; }
        public bool IsPopular { get; set; }
        public string? Avatar { get; set; }
        public string? Image { get; set; } 

    }
    public enum CreateArtistResult
    {
        Success,
        Error
    }
}
