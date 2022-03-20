using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Artists
{
    public class FilterArtistsDto:BasePaging
    {
        public string? FullName { get; set; }
        public List<Artist> Artists{ get; set; }
        public FilterArtistState FilterArtistState { get; set; }
        #region methods
        public FilterArtistsDto SetArtists(List<Artist> artists)
        {
            this.Artists = artists;
            return this;
        }
        public FilterArtistsDto SetPaging(BasePaging paging)
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
    public enum FilterArtistState
    {
        All,
        Popular,
        NotPopular
    }
}
