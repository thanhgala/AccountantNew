using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class ApplicationRoleGroupViewModel
    {
        public int GroupId { set; get; }

        public string RoleId { set; get; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

    }
}