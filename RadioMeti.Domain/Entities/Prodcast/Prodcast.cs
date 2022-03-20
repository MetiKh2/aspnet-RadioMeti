using RadioMeti.Domain.Entities.Base;
using RadioMeti.Domain.Entities.Prodcasts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Prodcast
{
    public class Prodcast : BaseEntityNullableDates
    {
        #region props
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public int PlaysCount { get; set; }
        [Display]
        public string? Description { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Narrator { get; set; }
        public string? Cover { get; set; }
        public long DjId { get; set; }

        #endregion        
        #region rel

        [ForeignKey("DjId")]
        public Dj Dj { get; set; }

        #endregion
    }
}
