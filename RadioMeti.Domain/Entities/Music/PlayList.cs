using Microsoft.AspNetCore.Identity;
using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Music
{
    public class PlayList: BaseEntityNullableDates
    {

        #region props
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public string? Cover { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Creator { get; set; }
        public string? UserId { get; set; }
        #endregion
        #region rel
        public IdentityUser? User{ get; set; }
        public ICollection<PlaylistMusic> PlaylistMusics { get; set; }
        public ICollection<PlayListSelectedCategory> PlayListSelectedCategories { get; set; }
        #endregion
    }
}
