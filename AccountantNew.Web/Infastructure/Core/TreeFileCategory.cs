using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Infastructure.Core
{
    public class TreeFileCategory<T>
    {
        public string text { set; get; }

        public string alias { set; get; }

        public int id { set; get; }

        public int? parentid { set; get; }

        public List<TreeFileCategory<T>> children { set; get; }

        public Dictionary<string, bool> state { set; get; }

    }
}