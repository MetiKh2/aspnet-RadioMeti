

using RadioMeti.Application.DTOs.Slider;

namespace RadioMeti.Application.DTOs.Event
{
    public class EventPageDto
    {
        public List<SiteSliderDto> Sliders { get; set; }
        public List<Domain.Entities.Event.Event> NewestEvents { get; set; }
    }
}
