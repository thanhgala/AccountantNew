using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    [Table("FocusNotifications")]
    public class FocusNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }

        public DateTime? EndDate { set; get; }

        public int Type { set; get; }

        [MaxLength(256)]
        public string Image { set; get; }

        public bool Status { set; get; }

    }
}
