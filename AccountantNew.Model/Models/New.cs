using AccountantNew.Model.Abstrack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    [Table("News")]
    public class New : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string Alias { set; get; }

        [Required]
        public int NewCategoryID { set; get; }

        [MaxLength(128)]
        [Required]
        public string AuthorID { set; get; }

        [MaxLength(500)]
        public string Title { set; get; }

        public string Content { set; get; }

        [MaxLength(256)]
        public string Image { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }

        public int? ViewCount { set; get; }

        public int? CommentCount { set; get; }


        public string Tags { set; get; }

        [ForeignKey("NewCategoryID")]
        public virtual NewCategory NewCategory { set; get; }

        [ForeignKey("AuthorID")]
        public virtual ApplicationUser User { set; get; }

        public virtual IEnumerable<NewTag> NewTags { set; get; }

        public virtual IEnumerable<Comment> Comments { set; get; }
    }
}
