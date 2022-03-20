using RadioMeti.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Domain.Entities.Music
{
    public class PlayListSelectedCategory : BaseEntityNullableDates
    {
        #region props
        public long PlayListId { get; set; }
        public long PlayListCategoryId { get; set; }
        #endregion
        #region rel
        [ForeignKey("PlayListCategoryId")]
        public PlayListCategory PlayListCategory { get; set; }
        [ForeignKey("PlayListId")]
        public PlayList PlayList { get; set; }
        #endregion
    }
}
