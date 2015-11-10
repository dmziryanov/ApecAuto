using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    internal enum DelayMode { Мilliseconds, Percentage }

    internal struct BatchParams
    {
        public int Size;
        public int Timeout;
        public int Delay;
        public DelayMode DelayMode;
    }
}
