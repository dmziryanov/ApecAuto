using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RmsAuto.Store.Acctg
{
    [Serializable]
    public class CurrencyRate
    {
        public static readonly CurrencyRate RurRate =
            new CurrencyRate
            {
                CurrencyCode = "usd.",
                CurrencyName = "U. S. Dollars",
                LastDate = DateTime.Now,
                Rate = 1.0m
            };
                  
        public string CurrencyCode { get; set; }

        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }

        public DateTime LastDate { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", this.CurrencyCode, this.Rate);
        }
    }
}
