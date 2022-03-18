using RadioMeti.Application.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Roles
{
    public class FilterRolesDto : BasePaging
    {
        public List<string> Roles{ get; set; }
        public string RoleName { get; set; }
        #region methods
        public FilterRolesDto SetRoles(List<string> roles)
        {
            this.Roles = roles;
            return this;
        }
        public FilterRolesDto SetPaging(BasePaging paging)
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
