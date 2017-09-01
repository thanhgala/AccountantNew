using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class PostViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage ="Bạn phải nhập tiêu đề bài viết")]
        public string Name { set; get; }

        public string Alias { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập vào nội dung")]
        public string Content { set; get; }

        public int NewCategoryID { set; get; }

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdateBy { set; get; }

        public bool Status { set; get; }

        public IEnumerable<CommentViewModel> Comments { set; get; }
    }
}