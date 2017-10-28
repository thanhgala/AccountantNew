using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    [Table("Organizationals")]
    public class Organizational
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public int ? ParentID { set; get; }

        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(150)]
        [Required]
        public string Position { set; get; }

        [StringLength(250)]
        public string Image { set; get; }
    }
}
