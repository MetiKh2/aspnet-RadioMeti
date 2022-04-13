

using RadioMeti.Application.DTOs.Paging;

namespace RadioMeti.Application.DTOs.Admin.Event
{
    public class FilterEventDto:BasePaging
    {
        public List<Domain.Entities.Event.Event>  Events{ get; set; }
        public FilterEventState FilterEventState { get; set; }
        public string? Title { get; set; }
        public string? HoldingTime { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public string? City { get; set; }
        public int? AgeLimit { get; set; }
        public string? WhenOpen { get; set; }
        public string? InformationPhone { get; set; }
        #region methods
        public FilterEventDto SetEvents(List<Domain.Entities.Event.Event> events)
        {
            this.Events = events;
            return this;
        }
        public FilterEventDto SetPaging(BasePaging paging)
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
    public enum FilterEventState
    {
        All,
        InSlider,
        DosentInSlider
    }
}
