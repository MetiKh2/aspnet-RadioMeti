using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Playlist.Category
{
    public class FilterPlaylistCategoryDto:BasePaging
    {
        public List<PlayListCategory> PlayListCategories{ get; set; }
        public string Title { get; set; }
        public FilterPlaylistCategoryState FilterPlaylistCategoryState { get; set; }
        #region methods
        public FilterPlaylistCategoryDto SetCategories(List<PlayListCategory> playListCategories)
        {
            this.PlayListCategories = playListCategories;
            return this;
        }
        public FilterPlaylistCategoryDto SetPaging(BasePaging paging)
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
    public enum FilterPlaylistCategoryState
    {
        All,
        InBrowse,
        NotInBrowse
    }
}
