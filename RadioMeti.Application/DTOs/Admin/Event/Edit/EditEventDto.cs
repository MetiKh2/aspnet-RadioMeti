

using RadioMeti.Application.DTOs.Common;

namespace RadioMeti.Application.DTOs.Admin.Event.Edit
{
    public class EditEventDto:CaptchaDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Cover { get; set; }
        public DateTime? HoldingDate { get; set; }
        public string? HoldingTime { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public string? City { get; set; }
        public int? AgeLimit { get; set; }
        public string? WhenOpen { get; set; }
        public string? InformationPhone { get; set; }
        public bool IsSlider { get; set; }
        public string? Description { get; set; }
    }
    public enum EditEventResult
    {
        Success,
        Error,
        Notfound
    }
}
