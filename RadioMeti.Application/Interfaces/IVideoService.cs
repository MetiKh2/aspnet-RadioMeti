
using RadioMeti.Application.DTOs.Admin.Video;
using RadioMeti.Application.DTOs.Admin.Video.Create;
using RadioMeti.Application.DTOs.Admin.Video.Delete;
using RadioMeti.Application.DTOs.Admin.Video.Edit;
using RadioMeti.Domain.Entities.Video;

namespace RadioMeti.Application.Interfaces
{
    public interface IVideoService
    {
        Task<Tuple<CreateVideoResult,long>> CreateVideo(CreateVideoDto create);
        Task<FilterVideoDto> FilterVideo(FilterVideoDto filter);
        Task CreateVideoArtists(long videoId, List<long> selectedArtists);
        Task DeleteVideoArtists(long videoId);
        Task<List<long>> GetVideoArtists(long videoId);
        Task<Video> GetVideoBy(long videoId);
        Task<EditVideoResult> EditVideo(EditVideoDto edit);
        Task<DeleteVideoResult> DeleteVideo(long id);
    }
}
