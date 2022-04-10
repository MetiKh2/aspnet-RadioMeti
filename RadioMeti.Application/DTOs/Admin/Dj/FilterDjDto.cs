using RadioMeti.Application.DTOs.Paging;
 
namespace RadioMeti.Application.DTOs.Admin.Dj
{
    public class FilterDjDto:BasePaging
    {
        public List<Domain.Entities.Prodcast.Dj> Djs { get; set; }
        public string FullName { get; set; }
        public string InstagramPage { get; set; }
        #region methods
        public FilterDjDto SetDjs(List<Domain.Entities.Prodcast.Dj> djs)
        {
            this.Djs= djs;
            return this;
        }
        public FilterDjDto SetPaging(BasePaging paging)
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

    
}
