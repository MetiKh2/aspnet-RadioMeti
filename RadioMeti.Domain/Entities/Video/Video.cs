using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Video
{
    public class Video: BaseEntityNullableDates
    {
        #region props
        [Required]
        [Display]
        [MaxLength(200)]
        public string Title { get; set; }
        public int PlaysCount { get; set; }
        public int LikesCount { get; set; }
        public string? Cover { get; set; }

        #endregion
        #region Rel
        public ICollection<ArtistVideo> ArtistVideos { get; set; }
        #endregion
    }
}
