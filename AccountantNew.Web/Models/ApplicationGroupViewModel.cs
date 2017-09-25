using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class ApplicationGroupViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public IEnumerable<ApplicationRoleViewModel> Roles { set; get; }

        public IEnumerable<int> NewCategory { set; get; }
    }
}