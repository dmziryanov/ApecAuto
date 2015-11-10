using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web
{
    public class ArticleAddedEventArgs : EventArgs
    {
        internal ArticleAddedEventArgs(SparePartPriceKey key, int qty)
        {
            PartKey = key;
            Qty = qty;
        }

        public SparePartPriceKey PartKey
        {
            get;
            private set;
        }

        public int Qty
        {
            get;
            private set;
        }
    }
}
