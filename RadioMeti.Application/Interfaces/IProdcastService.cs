using RadioMeti.Application.DTOs.Admin.Dj;
using RadioMeti.Application.DTOs.Admin.Dj.Create;
using RadioMeti.Application.DTOs.Admin.Dj.Delete;
using RadioMeti.Application.DTOs.Admin.Dj.Edit;
using RadioMeti.Application.DTOs.Admin.Music;
using RadioMeti.Application.DTOs.Admin.Prodcast;
using RadioMeti.Application.DTOs.Admin.Prodcast.Create;
using RadioMeti.Application.DTOs.Admin.Prodcast.Edit;
using RadioMeti.Domain.Entities.Prodcast;

namespace RadioMeti.Application.Interfaces
{
    public interface IProdcastService
    {
        #region dj
        Task<CreateDjResult> CreateDj(CreateDjDto create);
        Task<EditDjResult> EditDj(EditDjDto edit);
        Task<FilterDjDto> FilterDjs(FilterDjDto filter);
        Task<Dj> GetDjBy(long id);
        Task<bool> ExistDj(long id);
        Task<DeleteDjResult> DeleteDj(long id);
        #endregion

        #region Prodcast
        Task<FilterProdcastDto> FilterProdcasts(FilterProdcastDto filter);
        Task<Tuple<CreateProdcastResult, long>> CreateProdcast(CreateProdcastDto create);
        Task<Prodcast> GetProdcastBy(long id);
        Task<EditProdcastResult> EditProdcast(EditProdcastDto edit);
        Task<DeleteMusicResult> DeleteProdcast(long id);
        #endregion
    }
}
