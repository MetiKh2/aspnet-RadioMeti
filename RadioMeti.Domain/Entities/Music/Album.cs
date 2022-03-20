using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Music
{
    public class Album:BaseEntityNullableDates
    {
        #region props
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        public string? Cover { get; set; }
        #endregion
        #region rel
        public ICollection<ArtistAlbum> ArtistAlbums { get; set; }
        public ICollection<Music> Musics { get; set; }
        #endregion

    }
}
