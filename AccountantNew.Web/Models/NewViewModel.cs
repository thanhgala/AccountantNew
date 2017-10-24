using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class NewViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public int NewCategoryID { set; get; }

        public string ApplicationUserId { set; get; }

        public bool? Private { set; get; }

        public string Content { set; get; }

        public string Image { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }

        public int ViewCount { set; get; }

        public string Tags { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdateBy { set; get; }

        public bool Status { set; get; }

        public virtual NewCategoryViewModel NewCategory { set; get; }

        public virtual ApplicationUserViewModel ApplicationUser { set; get; }
    }
}