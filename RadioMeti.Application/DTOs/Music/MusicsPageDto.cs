

using RadioMeti.Application.DTOs.Slider;
using RadioMeti.Domain.Entities.Music;

namespace RadioMeti.Application.DTOs.Music
{
    public class MusicsPageDto
    {
        public List<SiteSliderDto> Sliders{ get; set; }
        public List<Domain.Entities.Music.Music> NewestMusics { get; set; }
        public List<Domain.Entities.Music.Music> PopularMusics { get; set; }
        public List<Domain.Entities.Music.Music> ThisMonthMusics { get; set; }
        public List<Domain.Entities.Music.Music> ThisWeekMusics { get; set; }
        public List<Domain.Entities.Music.Music> ThisDayMusics { get; set; }
        public List<Album> LastAlbums{ get; set; }
    }
}
