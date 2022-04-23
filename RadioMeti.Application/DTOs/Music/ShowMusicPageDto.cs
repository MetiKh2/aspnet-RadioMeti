 
namespace RadioMeti.Application.DTOs.Music
{
    public class ShowMusicPageDto
    {
        public Domain.Entities.Music.Music Music{ get; set; }
        public List<Domain.Entities.Music.Music> RelatedMusics{ get; set; }
    }
}
