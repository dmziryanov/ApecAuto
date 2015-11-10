using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    static class UserProfileDac
    {
        private static Func<StoreDataContext, int, IQueryable<UserProfileEntry>> _getEntriesByUserId =
            CompiledQuery.Compile((StoreDataContext dc, int userId) =>
                from entry in dc.UserProfileEntries where entry.UserID == userId select entry);

        private static Func<StoreDataContext, string, IQueryable<UserProfileEntry>> _getEntriesByUsername =
            CompiledQuery.Compile((StoreDataContext dc, string username) =>
                from u in dc.Users
                join e in dc.UserProfileEntries on u.UserID equals e.UserID
                where u.Username == username
                select e);
        
        public static UserProfileEntry GetProfileByUserId(int userId)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                return _getEntriesByUserId(dc.DataContext, userId).SingleOrDefault();
            }
        }

        public static UserProfileEntry GetProfileByUsername(string username)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                return _getEntriesByUsername(dc.DataContext, username).SingleOrDefault();
            }
        }

        public static void SaveProfile(
            int userId,
            IDictionary<string, object> propValues,
            DateTime activityTime)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                var entry = _getEntriesByUserId(dc.DataContext, userId).SingleOrDefault();
                                
                if (entry != null)
                {
                    entry.NameObjectMap = propValues;
                    entry.LastActivityTime = activityTime;
                    entry.LastUpdateTime = DateTime.Now;
                }
                else
                {
                    entry = new UserProfileEntry
                    {
                        UserID = userId,
                        NameObjectMap = propValues,
                        LastActivityTime = activityTime,
                        LastUpdateTime = DateTime.Now
                    };
                    dc.DataContext.UserProfileEntries.InsertOnSubmit(entry);
                }
                dc.DataContext.SubmitChanges(); 
            }
        }

        public static void UpdateActivityTime(int userId, DateTime activityTime)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                var entry = _getEntriesByUserId(dc.DataContext, userId).Single();
                entry.LastActivityTime = activityTime;
                dc.DataContext.SubmitChanges();
            }
        }
    }
}
