using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Common
{
    public class CaptchaDto
    {
        [Required]
        public string Captcha { get; set; }
    }
}
