using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Prodcast.Create
{
    public class CreateProdcastDto:CaptchaDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        [Display]
        public string? Description { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Narrator { get; set; }
        public string? Cover { get; set; }
        public long DjId { get; set; }
        public string? Audio { get; set; }
        public bool IsSlider { get; set; }
    }
    public enum CreateProdcastResult
    {
        Success,
        Error,
        DjNotfound
    }
}
