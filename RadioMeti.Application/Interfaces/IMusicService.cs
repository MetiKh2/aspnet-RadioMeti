using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Music.Album;
using RadioMeti.Application.DTOs.Admin.Music.Album.Create;
using RadioMeti.Application.DTOs.Admin.Music.Album.Delete;
using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single;
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.Interfaces
{
    public interface IMusicService
    {
        #region Single
        Task<Tuple<CreateSingleTrackResult, long>> CreateSingleTrack(CreateSingleTrackDto createSingleTrack);
        Task<EditSingleTrackResult> EditSingleTrack(EditSingleTrackDto editSingleTrack);
        Task<DeleteMusicResult> DeleteMusic(long id);

        #endregion
        #region Album
        Task<Tuple<CreateAlbumResult, long>> CreateAlbum(CreateAlbumDto createAlbum);
        Task<FilterAlbumDto> FilterAlbums(FilterAlbumDto filter);
        Task<Album> GetAlbumBy(long id);
        Task<List<long>> GetArtistsAlbum(long albumId);
        Task<EditAlbumResult> EditAlbum(EditAlbumDto editAlbum);
        Task<DeleteAlbumResult> DeleteAlbum(long id);
        #endregion
        Task<FilterMusicsDto> FilterMusics(FilterMusicsDto filter);
        Task<Music> GetMusicBy(long id);
        Task<List<long>> GetArtistsMusic(long musicId);

        #region ArtistMusic
        Task DeleteArtistsMusic(long musicId);
        Task CreateArtistsMusic(long musicId, List<long> artistsId);
        #endregion  
        #region ArtistAlbum
        Task DeleteArtistsAlbum(long albumId);
        Task CreateArtistsAlbum(long albumId, List<long> artistsId);
       
        #endregion
    }
}
