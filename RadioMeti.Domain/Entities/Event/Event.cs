using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Event
{
    public class Event:BaseEntityNullableDates
    {
        public string? Title { get; set; }
        public string? Cover { get; set; }
        public DateTime HoldingDate { get; set; }
        public string? HoldingTime { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public string? City { get; set; }
        public int? AgeLimit { get; set; }
        public string? WhenOpen { get; set; }
        public string? InformationPhone { get; set; }
        public bool IsSlider { get; set; }
        public string? Description { get; set; }
    }
}
