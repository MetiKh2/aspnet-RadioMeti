using RadioMeti.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace RadioMeti.Application.DTOs.Admin.Prodcast.Edit
{
    public class EditProdcastDto:CaptchaDto
    {
        public long Id { get; set; }
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
        public string? Audio { get; set; }
        public bool IsSlider { get; set; }
        public long DjId { get; set; }
    }
    public enum EditProdcastResult
    {
        Success,
        Error,
        ProdcastNotfound
    }
}
