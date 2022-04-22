using RadioMeti.Application.DTOs.Slider;

namespace RadioMeti.Application.DTOs.Prodcast
{
    public class ProdcastPageDto
    {
        public List<SiteSliderDto> Sliders { get; set; }
        public List<Domain.Entities.Prodcast.Prodcast> NewestProdcasts { get; set; }
        public List<Domain.Entities.Prodcast.Prodcast> PopularProdcasts { get; set; }
        public List<Domain.Entities.Prodcast.Prodcast> ThisMonthProdcasts { get; set; }
        public List<Domain.Entities.Prodcast.Prodcast> ThisWeekProdcasts { get; set; }
        public List<Domain.Entities.Prodcast.Prodcast> ThisDayProdcasts { get; set; }
    }
}
