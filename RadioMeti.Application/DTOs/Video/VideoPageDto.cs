using RadioMeti.Application.DTOs.Slider;

namespace RadioMeti.Application.DTOs.Video
{
    public class VideoPageDto
    {
        public List<SiteSliderDto> Sliders { get; set; }
        public List<Domain.Entities.Video.Video> NewestVideos { get; set; }
        public List<Domain.Entities.Video.Video> PopularVideos { get; set; }
        public List<Domain.Entities.Video.Video> ThisMonthVideos { get; set; }
        public List<Domain.Entities.Video.Video> ThisWeekVideos { get; set; }
        public List<Domain.Entities.Video.Video> ThisDayVideos { get; set; }
    }
}
