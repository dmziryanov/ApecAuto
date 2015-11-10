using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
    public enum ImportMode
    {
        /// <summary>
        /// Bulk insert based on Csv data (counting skipped rows is off)
        /// </summary>
        BulkInsert,

        /// <summary>
        /// Bulk insert based on Csv data (counting skipped rows is on)
        /// </summary>
        BulkInsertWithSkipped,
        
        /// <summary>
        /// Deletion based on Csv data
        /// </summary>
        BulkDelete,
        
        [Obsolete]
        Smart
    }
}
