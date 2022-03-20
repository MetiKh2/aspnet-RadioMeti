using RadioMeti.Domain.Entities.Base;
using RadioMeti.Domain.Entities.Video;
using System.ComponentModel.DataAnnotations;
 

namespace RadioMeti.Domain.Entities.Music
{
    public class Artist:BaseEntityNullableDates
    {

        #region props
        [Display]
        [MaxLength(200)]
        [Required]
        public string FullName { get; set; }
        [Display]
        public string? Avatar { get; set; }
        [Display]
        public string? Image { get; set; }
        #endregion
        #region rel
        public ICollection<ArtistAlbum> ArtistAlbums { get; set; }
        public ICollection<ArtistMusic> ArtistMusics { get; set; }
        public ICollection<ArtistVideo> ArtistVideos{ get; set; }
        #endregion

    }
}
