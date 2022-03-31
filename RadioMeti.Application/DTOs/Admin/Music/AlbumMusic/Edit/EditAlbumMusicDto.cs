using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Music.AlbumMusic.Edit
{
    public class EditAlbumMusicDto:EditSingleTrackDto
    {
        public long AlbumId { get; set; }
    }
}
