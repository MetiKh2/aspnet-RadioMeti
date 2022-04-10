using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Admin.Dj;
using RadioMeti.Application.DTOs.Admin.Dj.Create;
using RadioMeti.Application.DTOs.Admin.Dj.Delete;
using RadioMeti.Application.DTOs.Admin.Dj.Edit;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Prodcast;
using RadioMeti.Application.DTOs.Admin.Prodcast.Create;
using RadioMeti.Application.DTOs.Admin.Prodcast.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.Interfaces;
using RadioMeti.Domain.Entities.Prodcast;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.Application.Services
{

    public class ProdcastService: IProdcastService
    {
        private readonly IGenericRepository<Dj> _djRepository;
        private readonly IGenericRepository<Prodcast> _prodcastRepository;
        private readonly IMapper _mapper;

        public ProdcastService(IGenericRepository<Dj> djRepository, IGenericRepository<Prodcast> prodcastRepository, IMapper mapper)
        {
            _djRepository = djRepository;
            _prodcastRepository = prodcastRepository;
            _mapper = mapper;
        }

        #region dj

        public async Task<CreateDjResult> CreateDj(CreateDjDto create)
        {
            try
            {
                var dj = _mapper.Map<Dj>(create);
                await _djRepository.AddEntity(dj);
                await _prodcastRepository.SaveChangesAsync();
                return CreateDjResult.Success;
            }
            catch 
            {
                return CreateDjResult.Error;
            }
        }

        public async Task<DeleteDjResult> DeleteDj(long id)
        {
            try
            {
                var artist =await GetDjBy(id);
                if (artist == null) return DeleteDjResult.Notfound;
                _djRepository.DeleteEntity(artist);
                await _djRepository.SaveChangesAsync();
                return DeleteDjResult.Success;
            }
            catch
            {
                return DeleteDjResult.Error;
            }
        }

        public async Task<EditDjResult> EditDj(EditDjDto edit)
        {
            try
            {
                var dj = await _djRepository.GetEntityById(edit.Id);
                if (dj == null) return EditDjResult.Notfound;
                dj.UpdateDate = DateTime.Now;
                dj.FullName= edit.FullName;
                dj.InstagramPage = edit.InstagramPage;
                dj.Image = edit.Image;
                dj.Avatar = edit.Avatar;
                _djRepository.EditEntity(dj);
                await _djRepository.SaveChangesAsync();
                return EditDjResult.Success;
            }
            catch
            {
                return EditDjResult.Error;
            }
        }

        public async Task<bool> ExistDj(long id)
        {
            if (await _djRepository.GetQuery().AnyAsync(p => p.Id == id)) return true;
            return false;
        }


        public async Task<FilterDjDto> FilterDjs(FilterDjDto filter)
        {
            var query = _djRepository.GetQuery().OrderByDescending(p => p.CreateDate).AsQueryable();
            #region filter
            if (!string.IsNullOrEmpty(filter.FullName)) query = query.Where(p => p.FullName.Contains(filter.FullName)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.InstagramPage)) query = query.Where(p => p.InstagramPage.Contains(filter.InstagramPage)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allDjs= await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetDjs(allDjs).SetPaging(pager);
        }

        public async Task<Dj> GetDjBy(long id)
        {
            return await _djRepository.GetEntityById(id);
        }
        #endregion
        #region Prodcast
        public async Task<Tuple<CreateProdcastResult,long>> CreateProdcast(CreateProdcastDto create)
        {
            if (!await ExistDj(create.DjId)) return Tuple.Create(CreateProdcastResult.DjNotfound,Convert.ToInt64(0));
            try
            {
                var prodcast = _mapper.Map<Prodcast>(create);
                await _prodcastRepository.AddEntity(prodcast);
                await _prodcastRepository.SaveChangesAsync();
                return Tuple.Create(CreateProdcastResult.Success,create.DjId);
            }
            catch 
            {
                return Tuple.Create(CreateProdcastResult.Error, Convert.ToInt64(0));
            }
        }

        public async Task<FilterProdcastDto> FilterProdcasts(FilterProdcastDto filter)
        {
            var query = _prodcastRepository.GetQuery().OrderByDescending(p => p.CreateDate).AsQueryable();
            #region State
            switch (filter.FilterProdcastState)
            {
                case FilterProdcastState.All:
                    break;
                case FilterProdcastState.IsSlider:
                    query = query.Where(p => p.IsSlider).AsQueryable();
                    break;
                case FilterProdcastState.DosentSlider:
                    query = query.Where(p => !p.IsSlider).AsQueryable();
                    break;
            }
            #endregion
            #region filter
            if (filter.DjId!=null&&filter.DjId>0) query = query.Where(p => p.DjId==filter.DjId).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Narrator)) query = query.Where(p => p.Narrator.Contains(filter.Narrator)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allProdcasts = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetProdcasts(allProdcasts).SetPaging(pager);
        }

        public async Task<Prodcast> GetProdcastBy(long id)
        {
            return await _prodcastRepository.GetEntityById(id);
        }

        public async Task<EditProdcastResult> EditProdcast(EditProdcastDto edit)
        {
            var prodcast = await _prodcastRepository.GetEntityById(edit.Id);
            if (prodcast == null) return EditProdcastResult.ProdcastNotfound;
            try
            {
                prodcast.Title = edit.Title;
                prodcast.Narrator = edit.Narrator;  
                prodcast.Audio = edit.Audio;
                prodcast.Cover = edit.Cover;
                prodcast.UpdateDate = DateTime.Now;
                prodcast.Description = edit.Description;
                prodcast.IsSlider = edit.IsSlider;
                _prodcastRepository.EditEntity(prodcast);
                await _prodcastRepository.SaveChangesAsync();
                return EditProdcastResult.Success;
            }
            catch 
            {
                return EditProdcastResult.Error;
            }
        }

        public async Task<DeleteMusicResult> DeleteProdcast(long id)
        {
            try
            {
                var prodcast = await _prodcastRepository.GetEntityById(id);
                if (prodcast == null) return DeleteMusicResult.Notfound;
                _prodcastRepository.DeleteEntity(prodcast);
                await _prodcastRepository.SaveChangesAsync();
                return DeleteMusicResult.Success;
            }
            catch
            {
                return DeleteMusicResult.Error;
            }
        }
        #endregion
    }
}
