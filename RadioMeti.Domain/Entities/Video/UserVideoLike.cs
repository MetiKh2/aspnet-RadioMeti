using Microsoft.AspNetCore.Identity;
using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioMeti.Domain.Entities.Video
{
    public class UserVideoLike:BaseEntityNullableDates
    {
        [Key]
        public long Id { get; set; }
        public long VideoId { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(VideoId))]
        public Video Video{ get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
