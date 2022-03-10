using System.ComponentModel.DataAnnotations;
namespace RadioMeti.Domain.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    public class BaseEntityNullableDates
    {
        [Key]
        public long Id { get; set; }
        public bool? IsRemoved { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
