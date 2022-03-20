using RadioMeti.Domain.Entities.Base;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Video
{
    public class ArtistVideo:BaseEntityNullableDates
    {
        public long ArtistId { get; set; }
        public long VideoId { get; set; }

        [ForeignKey("ArtistId")]
        public Artist Artist{ get; set; }
        [ForeignKey("VideoId")]
        public Video Video{ get; set; }
    }
}
