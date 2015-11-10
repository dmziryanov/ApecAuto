using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    class LoadErrorEventArgs : EventArgs
    {
        public readonly Exception ErrorInfo;

        public LoadErrorEventArgs(Exception errorInfo)
        {
            if (errorInfo == null)
                throw new ArgumentNullException("errorInfo");
            ErrorInfo = errorInfo;
        }
    }
}
