using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    interface IDataLoader
    {
        void LoadData(IDataReader reader, out LoadCounters rowCounters);
    }
}
