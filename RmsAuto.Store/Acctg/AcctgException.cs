using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Acctg
{
    public class AcctgException : Exception
    {
        public AcctgException() { }

        public AcctgException(string message) : base(message) { }

        public AcctgException(string message, Exception inner) : base(message, inner) { }

        public AcctgError ErrorCode { get; set; }
    }
}
