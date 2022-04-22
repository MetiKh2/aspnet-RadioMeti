
using Microsoft.EntityFrameworkCore;
using RadioMeti.Application.DTOs.Admin.Video;
using RadioMeti.Application.DTOs.Admin.Video.Create;
using RadioMeti.Application.DTOs.Admin.Video.Delete;
using RadioMeti.Application.DTOs.Admin.Video.Edit;
using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Application.DTOs.Slider;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Utils;
using RadioMeti.Domain.Entities.Video;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.Application.Services
{
    public class VideoService: IVideoService
    {
        private readonly IGenericRepository<Video> _videoRepository;
        private readonly IGenericRepository<ArtistVideo> _artistVideoRepository;

        public VideoService(IGenericRepository<ArtistVideo> artistVideoRepository, IGenericRepository<Video> videoRepository)
        {
            _artistVideoRepository = artistVideoRepository;
            _videoRepository = videoRepository;
        }

        public async Task<Tuple<CreateVideoResult, long>> CreateVideo(CreateVideoDto create)
        {
            try
            {
                var video = new Video
                {
                    Title = create.Title,
                    Cover = create.Cover,
                    VideoFile = create.VideoFile,
                    IsSlider = create.IsSlider,
                };
                await _videoRepository.AddEntity(video);
                await _videoRepository.SaveChangesAsync();
                return Tuple.Create(CreateVideoResult.Success,video.Id);
            }
            catch 
            {
                return Tuple.Create(CreateVideoResult.Error, Convert.ToInt64(0));
            }
        }

        public async Task CreateVideoArtists(long videoId, List<long> selectedArtists)
        {
            foreach (var item in selectedArtists)
                await _artistVideoRepository.AddEntity(new ArtistVideo
                {
                    VideoId = videoId,
                    ArtistId=item
                });
            await _videoRepository.SaveChangesAsync();  
        }

        public async Task<DeleteVideoResult> DeleteVideo(long id)
        {
            try
            {
                var video = await _videoRepository.GetEntityById(id);
                if (video == null) return DeleteVideoResult.Notfound;
                _videoRepository.DeleteEntity(video);
                await _videoRepository.SaveChangesAsync();
                return DeleteVideoResult.Success;
            }
            catch
            {
                return DeleteVideoResult.Error;
            }
        }

        public async Task DeleteVideoArtists(long videoId)
        {
            foreach (var item in await _artistVideoRepository.GetQuery().Where(p=>p.VideoId==videoId).ToListAsync())
                 _artistVideoRepository.DeleteEntity(item);
            await _videoRepository.SaveChangesAsync();
        }


        public async Task<EditVideoResult> EditVideo(EditVideoDto edit)
        {
            var video = await _videoRepository.GetEntityById(edit.Id);
            if (video == null) return EditVideoResult.Notfound;
            try
            {
                video.Title = edit.Title;
                video.UpdateDate = DateTime.Now;
                video.IsSlider = edit.IsSlider;
                video.Cover = edit.Cover;
                video.VideoFile = edit.VideoFile;
                _videoRepository.EditEntity(video);
                await _videoRepository.SaveChangesAsync();
                return EditVideoResult.Success;
            }
            catch
            {
                return EditVideoResult.Error;
            }
        }

        public async Task<FilterVideoDto> FilterVideo(FilterVideoDto filter)
        {
            var query = _videoRepository.GetQuery().OrderByDescending(p => p.CreateDate).AsQueryable();
            #region State
            switch (filter.FilterVideoState)
            {
                case FilterVideoState.All:
                    break;
                case FilterVideoState.IsSlider:
                    query = query.Where(p => p.IsSlider).AsQueryable();
                    break;
                case FilterVideoState.DosentSlider:
                    query = query.Where(p => !p.IsSlider).AsQueryable();
                    break;
            }
            #endregion
            #region filter
            if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(p => p.Title.Contains(filter.Title)).AsQueryable();
            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allVideos= await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetVideos(allVideos).SetPaging(pager);
        }

        public async Task<List<SiteSliderDto>> GetInSliderVideos()
        {
            return await _videoRepository.GetQuery().Include(p => p.ArtistVideos).ThenInclude(p => p.Artist).Where(p => p.IsSlider && !string.IsNullOrEmpty(p.Cover)).Select(p => new SiteSliderDto
            {
                Title = p.Title,
                Cover = PathExtension.CoverVideoOriginPath + p.Cover,
                Artist = p.ArtistVideos.Select(p => p.Artist.FullName).ToList(),
                Id = p.Id
            }).ToListAsync();
        }

        public async Task<List<Video>> GetNewestVideos(int take)
        {
            return await _videoRepository.GetQuery().Include(p => p.ArtistVideos).ThenInclude(p=>p.Artist).Where(p => !string.IsNullOrEmpty(p.Cover)).OrderByDescending(p => p.CreateDate).Take(take).ToListAsync();
        }

        public async Task<List<Video>> GetPopularVideos(int take)
        {
            return await _videoRepository.GetQuery().Include(p => p.ArtistVideos).ThenInclude(p => p.Artist).Where(p => !string.IsNullOrEmpty(p.Cover)).OrderByDescending(p => p.LikesCount).Take(take).ToListAsync();
        }

        public async Task<List<long>> GetVideoArtists(long videoId)
        {
            return await _artistVideoRepository.GetQuery().Where(p => p.VideoId == videoId).Select(p => p.ArtistId).ToListAsync();
        }

        public async Task<Video> GetVideoBy(long videoId)
        {
            return await _videoRepository.GetEntityById(videoId);
        }

        public async Task<List<Video>> GetVideosByStartDate(int beforeDays, int take)
        {
            DateTime date = DateTime.Now.AddDays(-beforeDays);
            return await _videoRepository.GetQuery().Include(p => p.ArtistVideos).ThenInclude(p => p.Artist).Where(p => !string.IsNullOrEmpty(p.Cover) && p.CreateDate >= date).OrderBy(p => p.CreateDate).Take(take).ToListAsync();
        }
    }
}
