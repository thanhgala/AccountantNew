using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    public class ApplicationCateGroup
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int CategoryId { set; get; }

        [ForeignKey("GroupId")]
        public virtual ApplicationGroup ApplicationGroup { set; get; }

        [ForeignKey("CategoryId")]
        public virtual NewCategory NewCategory { set; get; }
    }
}
