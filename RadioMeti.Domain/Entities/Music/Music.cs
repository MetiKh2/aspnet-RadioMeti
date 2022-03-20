
using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioMeti.Domain.Entities.Music
{
    public class Music:BaseEntityNullableDates
    {

        #region props
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public long PlaysCount { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Poet { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Lyrics { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Arrangement { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Photographer { get; set; }
        [Display]
        [MaxLength(200)]
        public string? CoverArtist { get; set; }
        public string? Cover { get; set; }
        [Display]
        [MaxLength(200)]
        public string? MusicProducer { get; set; }
        public long LikesCount { get; set; }
        public bool IsSingle { get; set; }
        [Display]
        [MaxLength(200)]
        public string Tags { get; set; }

        public long? AlbumId { get; set; }
        #endregion
        #region Rel
        public ICollection<PlaylistMusic> PlaylistMusics { get; set; }
        [ForeignKey("AlbumId")]
        public Album? Album { get; set; }
        public ICollection<ArtistMusic> ArtistMusics { get; set; }
        #endregion

    }
}
