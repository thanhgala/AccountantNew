using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class FileViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public int NewCategoryID { set; get; }

        public string Path { set; get; }

        public string Describtion { set; get; }

        public DateTime TimeStarted { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdateBy { set; get; }

        public bool Status { set; get; }
    }
}