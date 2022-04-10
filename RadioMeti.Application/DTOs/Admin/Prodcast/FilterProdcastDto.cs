using RadioMeti.Application.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Prodcast
{
    public class FilterProdcastDto:BasePaging
    {
        public List<Domain.Entities.Prodcast.Prodcast>Prodcasts{ get; set; }
        public string Title { get; set; }
        public string Narrator { get; set; }
        public long DjId { get; set; }
        public FilterProdcastState FilterProdcastState { get; set; }
        #region methods
        public FilterProdcastDto SetProdcasts(List<Domain.Entities.Prodcast.Prodcast>  prodcasts)
        {
            this.Prodcasts = prodcasts;
            return this;
        }
        public FilterProdcastDto SetPaging(BasePaging paging)
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
    public enum FilterProdcastState
    {
        All,
        IsSlider,
        DosentSlider
    }
}
