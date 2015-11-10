using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class AddToCartResult
    {
        public bool isSuccessful;
        public string ErrorText;
        public decimal TotalSum;
        public int TotalCount;
    }
}
