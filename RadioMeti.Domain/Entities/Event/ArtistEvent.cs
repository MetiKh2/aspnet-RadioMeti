using RadioMeti.Domain.Entities.Base;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Event
{
    public class ArtistEvent:BaseEntityNullableDates
    {
        public long EventId { get; set; }
        public long ArtistId { get; set; }

        [ForeignKey("EventId")]
        public Event Event{ get; set; }
        [ForeignKey("ArtistId")]
        public Artist Artist{ get; set; }

    }
}
