using RadioMeti.Application.DTOs.Paging;
using RadioMeti.Domain.Entities;
namespace RadioMeti.Application.DTOs.Admin.Music.Single
{
    public class FilterMusicsDto:BasePaging
    {
        public string? Title { get; set; }
        public long? ArtistId { get; set; }
        public long? AlbumId { get; set; }
        public string? Poet { get; set; }
        public string? Lyrics { get; set; }
        public string? Arrangement { get; set; }
        public string? Photographer { get; set; }
        public string? CoverArtist { get; set; }
        public string? MusicProducer { get; set; }
        public List<Domain.Entities.Music.Music> Musics{ get; set; }
        public FilterMusicState FilterMusicState { get; set; }
        public bool IsSingle { get; set; }
        #region methods
        public FilterMusicsDto SetMusics(List<Domain.Entities.Music.Music> artists)
        {
            this.Musics = artists;
            return this;
        }
        public FilterMusicsDto SetPaging(BasePaging paging)
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
    public enum FilterMusicState
    {
        All,
        InSlider,
        DosentInSlider,
    }
}

