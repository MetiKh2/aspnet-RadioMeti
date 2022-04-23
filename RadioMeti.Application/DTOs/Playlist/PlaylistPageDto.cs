
using RadioMeti.Domain.Entities.Music;

namespace RadioMeti.Application.DTOs.Playlist
{
    public class PlaylistPageDto
    {
        public List<PlayList> FeaturedPlayLists { get; set; }
        public List<PlayListCategory> PlayListCategories { get; set; }
    }
}
