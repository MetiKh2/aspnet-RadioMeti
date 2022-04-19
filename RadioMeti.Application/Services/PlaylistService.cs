using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.DTOs.Playlist;
using RadioMeti.Application.DTOs.Playlist.Category;
using RadioMeti.Application.DTOs.Playlist.Category.Create;
using RadioMeti.Application.DTOs.Playlist.Category.Delete;
using RadioMeti.Application.DTOs.Playlist.Category.Edit;
using RadioMeti.Application.DTOs.Playlist.Create;
using RadioMeti.Application.DTOs.Playlist.Delete;
using RadioMeti.Application.DTOs.Playlist.Edit;
using RadioMeti.Application.Interfaces;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.Application.Services
{
    public class PlaylistService: IPlaylistService
    {
        private readonly IGenericRepository<PlayList> _playlistRepository;
        private readonly IGenericRepository<PlaylistMusic> _playlistMusicRepository;
        private readonly IGenericRepository<PlayListSelectedCategory> _playlistSelectedCategoryRepository;
        private readonly IGenericRepository<PlayListCategory> _playlistCategoryRepository;

        public PlaylistService(IGenericRepository<PlayListCategory> playlistCategoryRepository, IGenericRepository<PlayListSelectedCategory> playlistSelectedCategoryRepository, IGenericRepository<PlaylistMusic> playlistMusicRepository, IGenericRepository<PlayList> playlistRepository)
        {
            _playlistCategoryRepository = playlistCategoryRepository;
            _playlistSelectedCategoryRepository = playlistSelectedCategoryRepository;
            _playlistMusicRepository = playlistMusicRepository;
            _playlistRepository = playlistRepository;
        }



        #region Category
        public async Task<CreatePlaylistCategoryResult> CreateCategory(CreatePlaylistCategoryDto create)
        {
            try
            {
               await _playlistCategoryRepository.AddEntity(new PlayListCategory
                {
                    Cover = create.Cover,
                    Title = create.Title,
                    IsInBrowse = create.IsInBrowse,
                });
                await _playlistCategoryRepository.SaveChangesAsync();
                return CreatePlaylistCategoryResult.Success;
            }
            catch
            {
                return CreatePlaylistCategoryResult.Error;
            }
        }

        public async Task<DeleteCategoryResult> DeleteCategory(long id)
        {
            try
            {
                var playlist = await _playlistCategoryRepository.GetEntityById(id);
                if (playlist == null) return DeleteCategoryResult.Notfound;
                _playlistCategoryRepository.DeleteEntity(playlist);
                await _playlistCategoryRepository.SaveChangesAsync();
                return DeleteCategoryResult.Success;
            }
            catch
            {
                return DeleteCategoryResult.Error;
            }
        }

        public async Task<EditCategoryResult> EditCategory(EditPlaylistCategoryDto edit)
        {
            var category = await _playlistCategoryRepository.GetEntityById(edit.Id);
            if (category == null) return EditCategoryResult.CategoryNotfound;
            try
            {
               category.Title = edit.Title;
               category.IsInBrowse = edit.IsInBrowse;
               category.Cover = edit.Cover;
               category.UpdateDate = DateTime.Now;
                _playlistCategoryRepository.EditEntity(category);
                await _playlistCategoryRepository.SaveChangesAsync();
                return EditCategoryResult.Success;
            }
            catch
            {
                return EditCategoryResult.Error;
            }
        }


        public async Task<FilterPlaylistCategoryDto> FilterPlaylistCategory(FilterPlaylistCategoryDto filter)
        {
            var query = _playlistCategoryRepository.GetQuery().OrderByDescending(p => p.CreateDate).AsQueryable();
            #region State
            switch (filter.FilterPlaylistCategoryState)
            {
                case FilterPlaylistCategoryState.All:
                    break;
                case FilterPlaylistCategoryState.InBrowse:
                    query = query.Where(p => p.IsInBrowse).AsQueryable();
                    break;
                case FilterPlaylistCategoryState.NotInBrowse:
                    query = query.Where(p => !p.IsInBrowse).AsQueryable();
                    break;
            }
            #endregion
            #region filter
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allCategories = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetCategories(allCategories).SetPaging(pager);
        }

        public async Task<PlayListCategory> GetCategoryBy(long id)
        {
            return await _playlistCategoryRepository.GetEntityById(id);
        }

        public async Task<List<PlayListCategory>> GetPlayListCategories()
        {
            return await _playlistCategoryRepository.GetQuery().ToListAsync();
        }
        #endregion

        #region Playlist
        public async Task<FilterPlaylistDto> filterPlaylist(FilterPlaylistDto filter)
        {
            var query = _playlistRepository.GetQuery().OrderByDescending(p => p.CreateDate).AsQueryable();
             
            #region filter
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Creator)) query = query.Where(p => p.Creator.Contains(filter.Creator)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allPlaylists = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetPlaylists(allPlaylists).SetPaging(pager);
        }
        public async Task<EditPlaylistResult> EditPlaylist(EditPlaylistDto edit)
        {
            try
            {
                var playlist = await _playlistRepository.GetEntityById(edit.Id);
                if (playlist == null) return EditPlaylistResult.Notfound;
                playlist.Title = edit.Title;
                playlist.Creator = edit.Creator;
                playlist.Cover = edit.Cover;
                _playlistRepository.EditEntity(playlist);
                await _playlistRepository.SaveChangesAsync();
                return EditPlaylistResult.Success;
            }
            catch 
            {
                return EditPlaylistResult.Error;
            }
        }
        public async Task<Tuple<CreatePlaylistResult,long>> CreatePlaylist(CreatePlaylistDto create)
        {
            try
            {
                var playlist = new PlayList
                {
                    Cover = create.Cover,
                    Creator = create.Creator,
                    Title = create.Title,
                };
                await _playlistRepository.AddEntity(playlist);
                await _playlistRepository.SaveChangesAsync();
                return Tuple.Create(CreatePlaylistResult.Success,playlist.Id);
            }
            catch 
            {
                return Tuple.Create(CreatePlaylistResult.Error, Convert.ToInt64(0));
            }
        }
        public async Task<List<long>> GetPlaylistSelectedCategories(long playlistId)
        {
            return await _playlistSelectedCategoryRepository.GetQuery().Where(p => p.PlayListId == playlistId).Select(p => p.PlayListCategoryId).ToListAsync();
        }
        public async Task<PlayList> GetPlayListBy(long id)
        {
            return await _playlistRepository.GetEntityById(id);
        }
        public async Task<DeletePlaylistResult> DeletePlaylist(long id)
        {
            try
            {
                var playlist = await _playlistRepository.GetEntityById(id);
                if (playlist == null) return DeletePlaylistResult.Notfound;
                _playlistRepository.DeleteEntity(playlist);
                await _playlistRepository.SaveChangesAsync();
                return DeletePlaylistResult.Success;
            }
            catch
            {
                return DeletePlaylistResult.Error;
            }
        }
            #endregion

            #region Playlist selected categories
            public async Task DeletePlaylistSelectedCategories(long playlistId)
        {
            var playlistSelectedCategories = await _playlistSelectedCategoryRepository.GetQuery().Where(p => p.PlayListId == playlistId).ToListAsync();
            foreach (var item in playlistSelectedCategories)
             _playlistSelectedCategoryRepository.DeleteEntity(item);
            await _playlistSelectedCategoryRepository.SaveChangesAsync();
        }
        public async Task CreatePlaylistSelectedCategories(long playlistId, List<long> selectedCategories)
        {
            foreach (var item in selectedCategories)
                await _playlistSelectedCategoryRepository.AddEntity(new PlayListSelectedCategory
                {
                    PlayListId = playlistId,
                    PlayListCategoryId=item
                });
            await _playlistSelectedCategoryRepository.SaveChangesAsync();   
        }
        #endregion
        #region Playlist musics
        public async Task CreatePlaylistMusics(long playlistId, List<long> selectedMusics)
        {
            foreach (var item in selectedMusics)
                await _playlistMusicRepository.AddEntity(new PlaylistMusic
                {
                    PlayListId = playlistId,
                    MusicId= item
                });
            await _playlistMusicRepository.SaveChangesAsync();
        }
        public async Task<List<long>> GetPlaylistMusics(long playlistId)
        {
            return await _playlistMusicRepository.GetQuery().Where(p => p.PlayListId == playlistId).Select(p => p.MusicId).ToListAsync();
        }
        public async Task DeletePlaylistMusics(long playlistId)
        {
            var playlistMusics= await _playlistMusicRepository.GetQuery().Where(p => p.PlayListId == playlistId).ToListAsync();
            foreach (var item in playlistMusics)
                _playlistMusicRepository.DeleteEntity(item);
            await _playlistMusicRepository.SaveChangesAsync();
        }

        
        #endregion
    }
}
