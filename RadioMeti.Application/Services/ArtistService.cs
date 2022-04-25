using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Admin.Artists;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Delete;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.Interfaces;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.Application.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IGenericRepository<Artist> _artistRepository;
        private readonly IMapper _mapper;
        public ArtistService(IGenericRepository<Artist> artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<CreateArtistResult> CreateArtist(CreateArtistDto create)
        {
            try
            {
                var artist=_mapper.Map<Artist>(create);
                await _artistRepository.AddEntity(artist);
                await _artistRepository.SaveChangesAsync();
                return CreateArtistResult.Success;
            }
            catch 
            {
                return CreateArtistResult.Error;
                throw;
            }
        }

        public async Task<DeleteArtistResult> DeleteArtist(long id)
        {
            try
            {
                var artist = await GetArtistBy(id);
                if (artist == null) return DeleteArtistResult.ArtistNotfound;
                _artistRepository.DeleteEntity(artist);
                await _artistRepository.SaveChangesAsync();
                return DeleteArtistResult.Success;
            }
            catch
            {
                return DeleteArtistResult.Error;
            }
        }

        public async Task<EditArtistResult> EditArtist(EditArtistDto edit)
        {
            try
            {
                var artist = await _artistRepository.GetEntityById(edit.Id);
                if (artist == null) return EditArtistResult.ArtistNotfound;
                artist.UpdateDate = DateTime.Now;
                artist.IsPopular = edit.IsPopular;
                artist.FullName = edit.FullName;
                artist.Image = edit.Image;
                artist.Avatar = edit.Avatar;
                _artistRepository.EditEntity(artist);
                await _artistRepository.SaveChangesAsync();
                return EditArtistResult.Success;
            }
            catch
            {
                return EditArtistResult.Error;
            }
        }

        public async Task<FilterArtistsDto> FilterArtists(FilterArtistsDto filter)
        {
            var query = _artistRepository.GetQuery().OrderByDescending(p=>p.CreateDate).AsQueryable() ;
            #region state
            switch (filter.FilterArtistState)
            {
                case FilterArtistState.All:
                    break;
                case FilterArtistState.Popular:
                    query = query.Where(p => p.IsPopular).AsQueryable();
                    break;
                case FilterArtistState.NotPopular:
                    query = query.Where(p => !p.IsPopular).AsQueryable();
                    break;
                default:
                    break;
            }
            #endregion
            #region filter
            if (!string.IsNullOrEmpty(filter.FullName)) query = query.Where(p => p.FullName.Contains(filter.FullName)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allArtists= await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetArtists(allArtists).SetPaging(pager);
        }

        public async Task<Artist> GetArtistBy(long id)
        {
           return await _artistRepository.GetEntityById(id);
        }

        public async Task<Artist> GetArtistForSiteBy(long id)
        {
            return await _artistRepository.GetQuery().Include(p => p.ArtistMusics).ThenInclude(p => p.Music).
                Include(p => p.ArtistVideos).ThenInclude(p => p.Video).
                Include(p => p.ArtistAlbums).ThenInclude(p => p.Album).FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<List<Artist>> GetArtists()
        {
            return await _artistRepository.GetQuery().ToListAsync();
        }

        public async Task<List<Artist>> GetArtists(string query, int take)
        {
            return await _artistRepository.GetQuery().Where(p => p.FullName.Contains(query)).Take(take).ToListAsync();
        }
    }
}
