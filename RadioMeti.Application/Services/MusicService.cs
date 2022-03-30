using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Music.Album;
using RadioMeti.Application.DTOs.Admin.Music.Album.Create;
using RadioMeti.Application.DTOs.Admin.Music.Album.Delete;
using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single;
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.Interfaces;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.Application.Services
{
    public class MusicService : IMusicService
    {
        private readonly IGenericRepository<Music> _musicRepository;
        private readonly IGenericRepository<ArtistMusic> _artistMusicRepository;
        private readonly IGenericRepository<Artist> _artistRepository;
        private readonly IGenericRepository<Album> _albumRepository;
        private readonly IGenericRepository<ArtistAlbum> _artistAlbumRepository;
        private readonly IMapper _mapper;
        public MusicService(IGenericRepository<Music> musicRepository, IMapper mapper, IGenericRepository<ArtistMusic> artistMusicRepository, IGenericRepository<Artist> artistRepository, IGenericRepository<Album> albumRepository, IGenericRepository<ArtistAlbum> artistAlbumRepository)
        {
            _musicRepository = musicRepository;
            _mapper = mapper;
            _artistMusicRepository = artistMusicRepository;
            _artistRepository = artistRepository;
            _albumRepository = albumRepository;
            _artistAlbumRepository = artistAlbumRepository;
        }

        #region Album
        public async Task<DeleteAlbumResult> DeleteAlbum(long id)
        {
            try
            {
                var album = await _albumRepository.GetEntityById(id);
                if (album == null) return DeleteAlbumResult.Notfound;
                _albumRepository.DeleteEntity(album);
                await DeleteArtistsMusic(id);
                await _musicRepository.SaveChangesAsync();
                return DeleteAlbumResult.Success;
            }
            catch
            {
                return DeleteAlbumResult.Error;
            }
        }
        public async Task<EditAlbumResult> EditAlbum(EditAlbumDto editAlbum)
        {
            try
            {
                var album = await _albumRepository.GetEntityById(editAlbum.Id);
                if (album != null)
                {
                    album.Title = editAlbum.Title;
                    album.Cover= editAlbum.Cover;
                    album.IsSlider= editAlbum.IsSlider;
                    album.UpdateDate = DateTime.Now;
                    _albumRepository.EditEntity(album);
                    await _albumRepository.SaveChangesAsync();
                    return EditAlbumResult.Success;
                }
                return EditAlbumResult.AlbumNotfound;
            }
            catch
            {
                return EditAlbumResult.Error;
            }
        }
        public async Task<Album> GetAlbumBy(long id)
        {
            return await _albumRepository.GetEntityById(id);
        }

        public async Task<List<long>> GetArtistsAlbum(long albumId)
        {
            return await _artistAlbumRepository.GetQuery().Where(p => p.AlbumId == albumId).Select(p=>p.ArtistId).ToListAsync() ;
        }
        public async Task<Tuple<CreateAlbumResult, long>> CreateAlbum(CreateAlbumDto createAlbum)
        {
            try
            {
                var album = _mapper.Map<Album>(createAlbum);
                await _albumRepository.AddEntity(album);
                await _albumRepository.SaveChangesAsync();
                return Tuple.Create(CreateAlbumResult.Success, album.Id);
            }
            catch
            {
                return Tuple.Create(item1: CreateAlbumResult.Error, Convert.ToInt64(0));
            }
        }
        public async Task<FilterAlbumDto> FilterAlbums(FilterAlbumDto filter)
        {
            var query = _albumRepository.GetQuery().Include(p => p.ArtistAlbums).OrderByDescending(p => p.CreateDate).AsQueryable();
            if (filter.ArtistId != null && filter.ArtistId > 0)
            {
                query = _artistAlbumRepository.GetQuery().Where(p => p.ArtistId == filter.ArtistId).Select(p => p.Album).AsQueryable();
            }
            #region state
            switch (filter.FilterAlbumState)
            {
                case FilterAlbumState.All:
                    break;
                case FilterAlbumState.InSlider:
                    query = query.Where(p => p.IsSlider).AsQueryable();
                    break;
                case FilterAlbumState.DosentInSlider:
                    query = query.Where(p => !p.IsSlider).AsQueryable();
                    break;
                default:
                    break;
            }
            #endregion
            #region filter
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allMusics = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetAlbums(allMusics).SetPaging(pager);
        }

        #endregion
        #region ArtistAlbum
        public async Task CreateArtistsAlbum(long albumId, List<long> artistsId)
        {
            foreach (var item in artistsId)
            {
                await _artistAlbumRepository.AddEntity(new ArtistAlbum { AlbumId = albumId, ArtistId = item, });
            }
            await _artistMusicRepository.SaveChangesAsync();
        }
        public async Task DeleteArtistsAlbum(long albumId)
        {
            var artistsAlbum = await _artistAlbumRepository.GetQuery().Where(p => p.AlbumId == albumId).ToListAsync();
            foreach (var item in artistsAlbum)
                _artistAlbumRepository.DeleteEntity(item);
        }

        #endregion
        #region ArtistMusic
        public async Task CreateArtistsMusic(long musicId, List<long> artistsId)
        {
            foreach (var item in artistsId)
            {
                await _artistMusicRepository.AddEntity(new ArtistMusic { MusicId = musicId, ArtistId = item, });
            }
            await _artistMusicRepository.SaveChangesAsync();
        }
        public async Task DeleteArtistsMusic(long musicId)
        {
            var artistsMusic = await _artistMusicRepository.GetQuery().Where(p => p.MusicId == musicId).ToListAsync();
            foreach (var item in artistsMusic)
                _artistMusicRepository.DeleteEntity(item);
        }

        #endregion


        #region Single
        public async Task<Tuple<CreateSingleTrackResult,long>> CreateSingleTrack(CreateSingleTrackDto createSingleTrack)
        {
            try
            {
                    var track = _mapper.Map<Music>(createSingleTrack);
                    track.IsSingle = true;
                    await _musicRepository.AddEntity(track);
                    await _musicRepository.SaveChangesAsync();
                    return Tuple.Create(CreateSingleTrackResult.Success,track.Id);
            }
            catch
            {
                return Tuple.Create(item1: CreateSingleTrackResult.Error, Convert.ToInt64(0));
            }

        }

       
        public async Task<DeleteMusicResult> DeleteMusic(long id)
        {
            try
            {
                var music = await _musicRepository.GetEntityById(id);
                if (music == null) return DeleteMusicResult.Notfound;
                _musicRepository.DeleteEntity(music);
                await DeleteArtistsMusic(id);
                await _musicRepository.SaveChangesAsync();
                return DeleteMusicResult.Success;
            }
            catch
            {
                return DeleteMusicResult.Error;
            }
        }

        public async Task<EditSingleTrackResult> EditSingleTrack(EditSingleTrackDto editSingleTrack)
        {
            try
            {
                var track = await _musicRepository.GetEntityById(editSingleTrack.Id);
                if (track != null)
                {
                    track.IsSingle = true;
                    track.IsSlider = editSingleTrack.IsSlider;
                    track.Tags = editSingleTrack.Tags;
                    track.Arrangement = editSingleTrack.Arrangement;
                    track.MusicProducer = editSingleTrack.MusicProducer;
                    track.Photographer = editSingleTrack.Photographer;
                    track.Cover = editSingleTrack.Cover;
                    track.CoverArtist = editSingleTrack.CoverArtist;
                    track.Title = editSingleTrack.Title;
                    track.Poet = editSingleTrack.Poet;
                    track.Lyrics = editSingleTrack.Lyrics;
                    track.Audio = editSingleTrack.Audio; 
                    track.UpdateDate = DateTime.Now;
                    _musicRepository.EditEntity(track);
                    await _musicRepository.SaveChangesAsync();
                    return EditSingleTrackResult.Success;
                }
                return EditSingleTrackResult.MusicNotfound;
            }
            catch
            {
                return EditSingleTrackResult.Error;
            }
        }
        #endregion
        public async Task<FilterMusicsDto> FilterMusics(FilterMusicsDto filter)
        {
            var query = _musicRepository.GetQuery().Include(p => p.ArtistMusics).OrderByDescending(p => p.CreateDate).AsQueryable();
            if (filter.ArtistId != null && filter.ArtistId > 0)
            {
                query = _artistMusicRepository.GetQuery().Where(p => p.ArtistId == filter.ArtistId).Select(p => p.Music).AsQueryable();
            }
            #region state
            switch (filter.FilterMusicState)
            {
                case FilterMusicState.All:
                    break;
                case FilterMusicState.InSlider:
                    query = query.Where(p => p.IsSlider).AsQueryable();
                    break;
                case FilterMusicState.DosentInSlider:
                    query = query.Where(p => !p.IsSlider).AsQueryable();
                    break;
                default:
                    break;
            }
            #endregion
            #region filter
            if (filter.AlbumId != null && filter.AlbumId > 0) query = query.Where(p => p.AlbumId==filter.AlbumId).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Arrangement)) query = query.Where(p => p.Arrangement.Contains(filter.Arrangement)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.CoverArtist)) query = query.Where(p => p.CoverArtist.Contains(filter.CoverArtist)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Photographer)) query = query.Where(p => p.Photographer.Contains(filter.Photographer)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.MusicProducer)) query = query.Where(p => p.MusicProducer.Contains(filter.MusicProducer)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Poet)) query = query.Where(p => p.Poet.Contains(filter.Poet)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Lyrics)) query = query.Where(p => p.Lyrics.Contains(filter.Lyrics)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allMusics = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetMusics(allMusics).SetPaging(pager);
        }

        public async Task<List<long>> GetArtistsMusic(long musicId)
        {
            return await _artistMusicRepository.GetQuery().Where(p => p.MusicId == musicId).Select(p=>p.ArtistId).ToListAsync() ;
        }

        public async Task<Music> GetMusicBy(long id)
        {
            return await _musicRepository.GetEntityById(id);
        }

    
    }
}
