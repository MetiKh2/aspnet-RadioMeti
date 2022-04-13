
using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Video.Create
{
    public class CreateVideoDto:CaptchaDto
    {
        [Required]
        [Display]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Cover { get; set; }
        public string? VideoFile { get; set; }
        public bool IsSlider { get; set; }
    }
    public enum CreateVideoResult
    {
        Success,
        Error
    }
}
