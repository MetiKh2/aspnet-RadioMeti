

using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Video.Edit
{
    public class EditVideoDto:CaptchaDto
    {
        [Required]
        [Display]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Cover { get; set; }
        public string? VideoFile { get; set; }
        public bool IsSlider { get; set; }
        public long Id { get; set; }
    }
    public enum EditVideoResult
    {
        Success,
        Error,
        Notfound
    }
}
