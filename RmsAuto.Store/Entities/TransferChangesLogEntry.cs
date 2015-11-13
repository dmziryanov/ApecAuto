using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Entities
{
    public partial class TransferChangesLogEntry
    {
        public TimeSpan TransferDuration
        {
            get { return TimeSpan.FromMilliseconds(DurationInMilliseconds); }
        }

        public static TransferChangesLogEntry Load(int logEntryId)
        {
            using (var context = new DCFactory<StoreDataContext>())
            {
                return context.DataContext.TransferChangesLogEntries.Single(tcle => tcle.LogEntryID == logEntryId);
            }
        }
    }
}
