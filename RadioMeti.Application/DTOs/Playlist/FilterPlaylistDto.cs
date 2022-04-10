using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Playlist
{
    public class FilterPlaylistDto:BasePaging
    {
        public List<PlayList> PlayLists{ get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        #region methods
        public FilterPlaylistDto SetPlaylists(List<PlayList> playLists)
        {
            this.PlayLists = playLists;
            return this;
        }
        public FilterPlaylistDto SetPaging(BasePaging paging)
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
