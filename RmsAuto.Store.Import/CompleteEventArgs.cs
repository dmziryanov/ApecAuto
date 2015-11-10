using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class CompleteEventArgs : EventArgs
    {
        public LoadCounters Counters
        {
            get;
            internal set; 
        }
    }
}
