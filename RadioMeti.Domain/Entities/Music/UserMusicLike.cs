
using Microsoft.AspNetCore.Identity;
using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioMeti.Domain.Entities.Music
{
    public class UserMusicLike: BaseEntityNullableDates
    {
        [Key]
        public long Id { get; set; }
        public long MusicId { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(MusicId))]
        public Music Music { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User{ get; set; }
    }
}
