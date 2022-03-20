using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Event
{
    public class ArtistEvent:BaseEntityNullableDates
    {
        public long EventId { get; set; }
        public long ArtistId { get; set; }
    }
}
