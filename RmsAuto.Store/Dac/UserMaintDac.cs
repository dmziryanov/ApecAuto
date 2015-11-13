using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    public static class UserMaintDac
    {
        private static Func<StoreDataContext, Guid, IQueryable<UserMaintEntry>> _getEntries =
            CompiledQuery.Compile((StoreDataContext dc, Guid entryUid) =>
                from entry in dc.UserMaintEntries where entry.EntryUid == entryUid select entry);

        private static Func<StoreDataContext, string, IQueryable<UserMaintEntry>> _getEntriesByUsername =
            CompiledQuery.Compile((StoreDataContext dc, string username) =>
                from entry in dc.UserMaintEntries where entry.Username == username select entry);
                       
        public static void AddEntry(UserMaintEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.DataContext.UserMaintEntries.InsertOnSubmit(entry);
                dc.DataContext.SubmitChanges();
            }
        }

        public static UserMaintEntry GetEntry(Guid entryUid)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                return _getEntries(dc.DataContext, entryUid).SingleOrDefault();
            }
        }

        public static UserMaintEntry[] GetEntries(string username)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                return _getEntriesByUsername(dc.DataContext, username).ToArray();
            }
        }


        internal static void SetEntryTime(Guid entryUid, DateTime entryTime)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                _getEntries(dc.DataContext, entryUid).Single().EntryTime = entryTime;
                dc.DataContext.SubmitChanges();
            }
        }
        
        internal static void DeleteEntry(Guid entryUid, StoreDataContext context)
        {
            context.UserMaintEntries.DeleteOnSubmit(_getEntries(context, entryUid).Single());
            context.SubmitChanges();
        }
    }
}
