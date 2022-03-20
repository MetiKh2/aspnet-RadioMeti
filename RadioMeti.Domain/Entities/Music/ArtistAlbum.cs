using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Music
{
    public class ArtistAlbum: BaseEntityNullableDates
    {
        public long ArtistId { get; set; }
        public long AlbumId { get; set; }

        #region rel
        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }
        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
        #endregion
    }
}
