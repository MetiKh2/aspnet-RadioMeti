using RadioMeti.Application.DTOs.Paging;

namespace RadioMeti.Application.DTOs.Admin.Users
{
    public class FilterUsersDto:BasePaging
    {
        public List<UsersListDto> Users { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public FilterUsersState FilterUsersState { get; set; }
        #region methods
        public FilterUsersDto SetUsers(List<UsersListDto> users)
        {
            this.Users =users;
            return this;
        }
        public FilterUsersDto SetPaging(BasePaging paging)
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

    public enum FilterUsersState
    {
        All,
        EmailConfrimed,
        EmailNotConfrimed,
    }
}
