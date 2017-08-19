using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Models
{
    public class CurrencyModel
    {
        public string CurrencyName { get; set; }

        public double Buy { get; set; }

        public double Sell { get; set; }

        public string CurrencyCode { get; set; }
    }
}