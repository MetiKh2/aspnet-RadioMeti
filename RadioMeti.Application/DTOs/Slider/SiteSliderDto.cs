namespace RadioMeti.Application.DTOs.Slider
{
    public class SiteSliderDto
    {
        public long Id { get; set; }
        public string? Cover { get; set; }
        public string? Title { get; set; }
        public List<string> Artist { get; set; }
    }
}
