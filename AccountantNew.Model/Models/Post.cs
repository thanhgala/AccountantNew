﻿using AccountantNew.Model.Abstrack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Model.Models
{
    [Table("Posts")]
    public class Post : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        [Required]
        public string Alias { set; get; }

        public string Content { set; get; }

        [Required]
        public int NewCategoryID { set; get; }

        public int ViewCount { set; get; }

        [MaxLength(128)]
        public string ApplicationUserId { set; get; }

        [ForeignKey("NewCategoryID")]
        public virtual NewCategory NewCategory { set; get; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { set; get; }


        public virtual IEnumerable<Comment> Comments { set; get; }

    }
}