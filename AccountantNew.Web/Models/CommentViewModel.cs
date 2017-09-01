using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class CommentViewModel
    {
        public int ID { set; get; }

        [Required]
        public string Content { set; get; }

        public string UserID { set; get; }

        public int PostID { set; get; }

        public DateTime CreateDate { set; get; }
    }
}