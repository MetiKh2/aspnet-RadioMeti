
using RadioMeti.Application.DTOs.Paging;

namespace RadioMeti.Application.DTOs.Admin.Video
{
    public class FilterVideoDto : BasePaging
    {
        public List<Domain.Entities.Video.Video> Videos{ get; set; }
        public string Title { get; set; }
        public FilterVideoState FilterVideoState { get; set; }
        #region methods
        public FilterVideoDto SetVideos(List<Domain.Entities.Video.Video> videos)
        {
            this.Videos = videos;
            return this;
        }
        public FilterVideoDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.EndPage = paging.EndPage;
            this.StartPage = paging.StartPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.TakeEntity = paging.TakeEntity;
            this.PageCount = paging.PageCount;
            return this;
        }
        #endregion

    }
    public enum FilterVideoState
    {
        All,
        IsSlider,
        DosentSlider
    }

}
