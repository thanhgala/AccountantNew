using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class ApplicationRoleViewModel
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public ApplicationRoleGroupViewModel ApplicationRoleGroup { set; get; }

    }
}