using RadioMeti.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.DTOs.Admin.Music.Single.Edit
{
    public class EditSingleTrackDto
    {
        [Display]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Poet { get; set; }
        [Display]
        public string? Lyrics { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Arrangement { get; set; }
        [Display]
        [MaxLength(200)]
        public string? Photographer { get; set; }
        [Display]
        [MaxLength(200)]
        public string? CoverArtist { get; set; }
        public string? Cover { get; set; }
        [Display]
        [MaxLength(200)]
        public string? MusicProducer { get; set; }

        [Display]
        [MaxLength(200)]
        public string? Tags { get; set; }
        public long Id { get; set; }
        public bool IsSlider { get; set; }
        public string? Audio{ get; set; }

    }
    public enum EditSingleTrackResult
    {
        Success,
        Error,
        MusicNotfound,
    }
}

