using AccountantNew.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AccountantNew.Web.Infastructure.Core
{
    public class XmlHelper
    {
        public static List<CurrencyModel> ParseXmlData(List<CurrencyModel> listCurrency)
        {
            XDocument xdoc = XDocument.Load("https://www.vietcombank.com.vn/ExchangeRates/ExrateXML.aspx");
            var xdata = from data in xdoc.Root.Elements("Exrate")
                        where (string)data.Attribute("CurrencyCode") == "THB" ||
                        (string)data.Attribute("CurrencyCode") == "USD" ||
                        (string)data.Attribute("CurrencyCode") == "JPY" ||
                        (string)data.Attribute("CurrencyCode") == "EUR"
                        select data;
            foreach (var item in xdata)
            {
                listCurrency.Add(new CurrencyModel()
                {
                    CurrencyName = (string)item.LastAttribute.Parent.Attribute("CurrencyName"),
                    Buy = (double)item.LastAttribute.Parent.Attribute("Buy"),
                    Sell = (double)item.LastAttribute.Parent.Attribute("Sell"),
                    CurrencyCode = (string)item.LastAttribute.Parent.Attribute("CurrencyCode")
                });
            }
            return listCurrency;
        }
    }
}