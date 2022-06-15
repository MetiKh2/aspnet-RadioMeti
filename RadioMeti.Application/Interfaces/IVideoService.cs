
using RadioMeti.Application.DTOs.Admin.Video;
using RadioMeti.Application.DTOs.Admin.Video.Create;
using RadioMeti.Application.DTOs.Admin.Video.Delete;
using RadioMeti.Application.DTOs.Admin.Video.Edit;
using RadioMeti.Application.DTOs.Slider;
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
        Task<List<SiteSliderDto>> GetInSliderVideos();
        Task<List<Video>> GetNewestVideos(int take);
        Task<List<Video>> GetPopularVideos(int take);
        Task<bool> AddLikeVideo(int id,string userId);
        Task<List<Video>> GetVideosByStartDate(int beforeDays, int take);
        Task<Video> GetVideoForSiteBy(long id);
        Task AddPlaysVideo(Video video);
        Task<List<Video>> GetRelatedVideos(Video video);
        Task<List<Video>> GetAllVideosForSite();
        Task<List<Video>> GetVideos(string query,int take);
    }
}
