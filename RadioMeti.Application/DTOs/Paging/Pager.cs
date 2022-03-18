using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageId, int allEntitiesCount, int take, int HowManyShowPageAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)take));
            return new BasePaging
            {
                PageId = pageId,
                AllEntitiesCount = allEntitiesCount,
                HowManyShowPageAfterAndBefore = HowManyShowPageAfterAndBefore,
                TakeEntity = take,
                SkipEntity = (pageId - 1) * take,
                StartPage = pageId - HowManyShowPageAfterAndBefore <= 0 ? 1 : pageId - HowManyShowPageAfterAndBefore,
                EndPage = pageId + HowManyShowPageAfterAndBefore > pageCount ? pageCount : pageId + HowManyShowPageAfterAndBefore,
                PageCount = pageCount
            };
        }
    }
}
