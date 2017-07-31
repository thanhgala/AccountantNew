using AccountantNew.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Infastructure.Core
{
    public class TreeViewCategory<T>
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public int? ParentID { set; get; }

        public int? DisplayOrder { set; get; }

        public List<TreeViewCategory<NewCategoryViewModel>> Nodes { set; get; }


    }
}