using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    public static class ManagerDac
    {
        private static Func<StoreDataContext, int, IQueryable<HandyClientSetEntry>> _getHandySetEntries =
            CompiledQuery.Compile((StoreDataContext dc, int managerId) =>
                from entry in dc.HandyClientSetEntries where entry.ManagerID == managerId select entry);

        public static HandyClientSetEntry[] GetHandySetEntries(int managerId)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                return _getHandySetEntries(dc.DataContext, managerId).ToArray();
            }
        }

        public static void AddHandySetEntry(HandyClientSetEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            using (var context = new DCFactory<StoreDataContext>())
            {
                context.DataContext.HandyClientSetEntries.InsertOnSubmit(entry);
                if (entry.IsDefault)
                {
                    var oldDefault = _getHandySetEntries(context.DataContext, entry.ManagerID)
                        .SingleOrDefault(e => e.IsDefault);
                    if (oldDefault != null)
                        oldDefault.IsDefault = false;
                }
                context.DataContext.SubmitChanges();
            }
        }

        public static void SetHandySetEntryAsDefault(int managerId, string clientId)
        {
            using (var context = new DCFactory<StoreDataContext>())
            {
                var hcse = _getHandySetEntries(context.DataContext, managerId).Single(e => e.ClientID == clientId);
                if (!hcse.IsDefault)
                {
                    var oldDefault = _getHandySetEntries(context.DataContext, managerId).SingleOrDefault(e => e.IsDefault);
                    if (oldDefault != null)
                        oldDefault.IsDefault = false;
                }
                hcse.IsDefault = true;
                context.DataContext.SubmitChanges();
            }
        }

        public static void DeleteHandySetEntry(int managerId, string clientId)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.DataContext.HandyClientSetEntries.DeleteOnSubmit(
                    _getHandySetEntries(dc.DataContext, managerId)
                    .Single(e => e.ClientID == clientId));
                dc.DataContext.SubmitChanges();
            }
        }

        public static void DeleteAllHandySetEntries(int managerId)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                dc.DataContext.HandyClientSetEntries.DeleteAllOnSubmit(
                    _getHandySetEntries(dc.DataContext, managerId));
                dc.DataContext.SubmitChanges();
            }
        }

        public static HandyClientSetEntry GetDefaultHandySetEntry(int managerId)
        {
            using (var dc = new DCFactory<StoreDataContext>())
            {
                return _getHandySetEntries(dc.DataContext, managerId).SingleOrDefault(e => e.IsDefault);
            }
        }
    }
}
