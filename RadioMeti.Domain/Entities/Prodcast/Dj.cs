using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Prodcast
{
    public class Dj:BaseEntityNullableDates
    {
        #region props
        [Display]
        [MaxLength(200)]
        [Required]
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Image { get; set; }
        [Display]
        [MaxLength(200)]
        public string? InstagramPage { get; set; }

        #endregion
        #region rel
        public ICollection<Prodcast> Prodcasts { get; set; }
        #endregion
    }
}
