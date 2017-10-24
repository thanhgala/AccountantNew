using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<NewViewModel> HotNews { set; get; }

        public IEnumerable<NewViewModel> LatestNews { set; get; }

        public IEnumerable<NewViewModel> Notifications { set; get; }

        public IEnumerable<CurrencyModel> CurrencyRate { set; get; }
    }
}