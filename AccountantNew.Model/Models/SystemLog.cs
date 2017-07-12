using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    [Table("SystemLogs")]
    public class SystemLog
    {
        [Key]
        public Guid SystemLogID { set; get; }

        [MaxLength(128)]
        public string ApplicationUserId { set; get; }

        [MaxLength(200)]
        public string DescriptionAction { set; get; }

        [Required]
        public DateTime LogDate { set; get; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { set; get; }
    }
}
