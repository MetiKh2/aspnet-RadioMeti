using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioMeti.Domain.Entities.Music
{
    public class ArtistMusic:BaseEntityNullableDates
    {
        #region props
        public long ArtistId { get; set; }
        public long MusicId { get; set; }

        #endregion
        #region rels
        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }
        [ForeignKey("MusicId")]
        public Music Music { get; set; }
        #endregion
    }
}
