using RadioMeti.Application.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Music.Album
{
    public class FilterAlbumDto:BasePaging
    {
        public string? Title { get; set; }
        public long? ArtistId { get; set; }
        public long? AlbumId { get; set; }
        public List<Domain.Entities.Music.Album> Albums{ get; set; }
        public FilterAlbumState  FilterAlbumState { get; set; }
        #region methods
        public FilterAlbumDto SetAlbums(List<Domain.Entities.Music.Album> albums)
        {
            this.Albums = albums;
            return this;
        }
        public FilterAlbumDto SetPaging(BasePaging paging)
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
    public enum FilterAlbumState
    {
        All,
        InSlider,
        DosentInSlider,
    }
}
