using RadioMeti.Domain.Entities.Music;

namespace RadioMeti.Application.DTOs.Search
{
    public class SearchPageDto
    {
        public List<Artist> Artists{ get; set; }
        public List<Domain.Entities.Video.Video> Videos{ get; set; }
        public List<Domain.Entities.Prodcast.Prodcast>  Prodcasts{ get; set; }
        public List<Domain.Entities.Music.Music> Musics{ get; set; }
        public List<Domain.Entities.Music.Album> Albums{ get; set; }
        public List<Domain.Entities.Prodcast.Dj>Djs{ get; set; }
        public List<Domain.Entities.Music.PlayList>PlayLists{ get; set; }
    }
}
