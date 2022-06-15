
using Microsoft.AspNetCore.Identity;
using RadioMeti.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioMeti.Domain.Entities.Prodcast
{
    public class UserProdcastLike: BaseEntityNullableDates
    {
        [Key]
        public long Id { get; set; }
        public long ProdcastId { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(ProdcastId))]
        public Prodcast Prodcast { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
