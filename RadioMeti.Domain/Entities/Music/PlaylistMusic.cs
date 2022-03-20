using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioMeti.Domain.Entities.Music
{
    public class PlaylistMusic:BaseEntityNullableDates
    {
        #region props
        public long MusicId { get; set; }
        public long PlayListId { get; set; }

        #endregion
        #region rel
        [ForeignKey("MusicId")]
        public Music Music { get; set; }
        [ForeignKey("PlayListId")]
        public PlayList PlayList { get; set; }
        #endregion
    }
}
