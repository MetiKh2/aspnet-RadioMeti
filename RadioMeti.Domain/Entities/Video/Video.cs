using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
 

namespace RadioMeti.Domain.Entities.Video
{
    public class Video: BaseEntityNullableDates
    {
        #region props
        [Required]
        [Display]
        [MaxLength(200)]
        public string Title { get; set; }
        public int PlaysCount { get; set; }
        public int LikesCount { get; set; }
        public string? Cover { get; set; }
        public string? VideoFile { get; set; }
        public bool IsSlider { get; set; }

        #endregion
        #region Rel
        public ICollection<ArtistVideo> ArtistVideos { get; set; }
        #endregion
    }
}
