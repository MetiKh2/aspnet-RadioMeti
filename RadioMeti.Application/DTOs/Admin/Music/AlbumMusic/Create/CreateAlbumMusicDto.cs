
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;

namespace RadioMeti.Application.DTOs.Admin.Music.AlbumMusic.Create
{
    public class CreateAlbumMusicDto:CreateSingleTrackDto
    {
        public long AlbumId { get; set; }
    }
    public enum CreateAlbumMusicResult
    {
        Success,
        Error,
        AlbumNotfound
    }
}
