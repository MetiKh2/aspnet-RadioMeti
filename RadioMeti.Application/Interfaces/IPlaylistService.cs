
using RadioMeti.Application.DTOs.Playlist;
using RadioMeti.Application.DTOs.Playlist.Category;
using RadioMeti.Application.DTOs.Playlist.Category.Create;
using RadioMeti.Application.DTOs.Playlist.Category.Delete;
using RadioMeti.Application.DTOs.Playlist.Category.Edit;
using RadioMeti.Application.DTOs.Playlist.Create;
using RadioMeti.Application.DTOs.Playlist.Delete;
using RadioMeti.Application.DTOs.Playlist.Edit;
using RadioMeti.Domain.Entities.Music;

namespace RadioMeti.Application.Interfaces
{
    public interface IPlaylistService
    {
        #region Category
        Task<CreatePlaylistCategoryResult> CreateCategory(CreatePlaylistCategoryDto create);
        Task<FilterPlaylistCategoryDto> FilterPlaylistCategory(FilterPlaylistCategoryDto filter);
        Task<PlayListCategory> GetCategoryBy(long id);
        Task<EditCategoryResult> EditCategory(EditPlaylistCategoryDto edit);
        Task<DeleteCategoryResult> DeleteCategory(long id);
        Task<List<PlayListCategory>> GetPlayListCategories();
        #endregion
        #region Playlist
        Task<FilterPlaylistDto> filterPlaylist(FilterPlaylistDto filter);
        Task<Tuple<CreatePlaylistResult, long>> CreatePlaylist(CreatePlaylistDto create);
        Task<PlayList> GetPlayListBy(long id);
        Task<EditPlaylistResult> EditPlaylist(EditPlaylistDto edit);
        Task<DeletePlaylistResult> DeletePlaylist(long id);
        Task<PlayList> GetPlayListForSiteBy(long id);
        Task<List<PlayList>> GetFeaturedPlayLists();
        Task<List<PlayList>> GetPlaylistsByCategory(long categoryId);
        #endregion
        #region Playlist selected categories
        Task CreatePlaylistSelectedCategories(long playlistId, List<long> selectedCategories);
        Task DeletePlaylistSelectedCategories(long playlistId);
        Task<List<long>> GetPlaylistSelectedCategories(long playlistId);
        #endregion 
        #region Playlist musics
        Task CreatePlaylistMusics(long playlistId, List<long> selectedMusics);
        Task DeletePlaylistMusics(long playlistId);
        Task<List<long>> GetPlaylistMusics(long playlistId);
        #endregion
    }
}
