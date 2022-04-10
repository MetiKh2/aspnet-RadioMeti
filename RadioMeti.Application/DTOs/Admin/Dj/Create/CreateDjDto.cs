using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Dj.Create
{
    public class CreateDjDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Image { get; set; }
        [Display]
        [MaxLength(200)]
        public string? InstagramPage { get; set; }
    }
    public enum CreateDjResult
    {
        Success,
        Error
    }
}
