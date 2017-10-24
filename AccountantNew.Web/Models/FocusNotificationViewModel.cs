using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class FocusNotificationViewModel
    {
        public int ID { set; get; }

        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }

        public DateTime? EndDate { set; get; }

        public int Type { set; get; }

        public string Image { set; get; }

        public bool Status { set; get; }
    }
}