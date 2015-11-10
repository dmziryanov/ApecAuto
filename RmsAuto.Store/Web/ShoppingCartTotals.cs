using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Web
{
    public class ShoppingCartTotals
    {
        public decimal Total { get; internal set; }

        public int PartsCount { get; internal set; }

        public int ItemsCount { get; internal set; }
    }
}
