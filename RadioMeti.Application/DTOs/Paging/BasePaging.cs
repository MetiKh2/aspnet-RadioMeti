using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Paging
{
   public class BasePaging
    {
        public BasePaging()
        {
            PageId = 1;
            TakeEntity = 5;
            HowManyShowPageAfterAndBefore = 3;
        }
        public int PageId { get; set; }
        public int PageCount { get; set; }
        public int AllEntitiesCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int TakeEntity { get; set; }
        public int SkipEntity { get; set; }
        public int HowManyShowPageAfterAndBefore { get; set; }
        public string GetCurrentPagingStatus()
        {
            return $"نمایش {((PageId * TakeEntity)-TakeEntity)+1}-{(PageId * TakeEntity>AllEntitiesCount?AllEntitiesCount:PageId * TakeEntity)} از  {AllEntitiesCount}";
        }
        public BasePaging GetCurrentPaging()
        {
            return this;
        }
    }
}
