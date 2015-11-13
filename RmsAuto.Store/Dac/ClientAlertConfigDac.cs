using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Dac
{
    [Obsolete]
	public static class ClientAlertConfigDac
	{
		//private static Func<StoreDataContext, int, IQueryable<UserProfileEntry>> _getEntriesByUserId =
		//    CompiledQuery.Compile((StoreDataContext dc, int userId) =>
		//        from entry in dc.UserProfileEntries where entry.UserID == userId select entry);
		private static Func<StoreDataContext, string, IQueryable<ClientAlertConfig>> _getAlertConfigByClientId =
			CompiledQuery.Compile((StoreDataContext dc, string clientId) =>
				from alertConfig in dc.ClientAlertConfigs where alertConfig.ClientID == clientId select alertConfig);

		//public static UserProfileEntry GetProfileByUserId(int userId)
		//{
		//    using (var dc = new StoreDataContext())
		//    {
		//        return _getEntriesByUserId(dc, userId).SingleOrDefault();
		//    }
		//}
		public static ClientAlertConfig GetAlertConfigByClientId(string clientId)
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				return _getAlertConfigByClientId(dc.DataContext, clientId).SingleOrDefault();
			}
		}

		public static void SaveClientAlertConfig(
			string clientId,
			int hourOfPeriod,
			string statusIds)
		{
            using (var dc = new DCFactory<StoreDataContext>())
			{
				var alertConfig = _getAlertConfigByClientId(dc.DataContext, clientId).SingleOrDefault();

				if (alertConfig != null)
				{
					alertConfig.HourOfPeriod = hourOfPeriod;
					alertConfig.StatusIDs = statusIds;
				}
				else
				{
					alertConfig = new ClientAlertConfig
					{
						ClientID = clientId,
						HourOfPeriod = hourOfPeriod,
						StatusIDs = statusIds
					};
					dc.DataContext.ClientAlertConfigs.InsertOnSubmit(alertConfig);
				}
				dc.DataContext.SubmitChanges();
			}
		}

		//public static void SaveProfile(
		//    int userId,
		//    IDictionary<string, object> propValues,
		//    DateTime activityTime)
		//{
		//    using (var dc = new StoreDataContext())
		//    {
		//        var entry = _getEntriesByUserId(dc, userId).SingleOrDefault();

		//        if (entry != null)
		//        {
		//            entry.NameObjectMap = propValues;
		//            entry.LastActivityTime = activityTime;
		//            entry.LastUpdateTime = DateTime.Now;
		//        }
		//        else
		//        {
		//            entry = new UserProfileEntry
		//            {
		//                UserID = userId,
		//                NameObjectMap = propValues,
		//                LastActivityTime = activityTime,
		//                LastUpdateTime = DateTime.Now
		//            };
		//            dc.UserProfileEntries.InsertOnSubmit(entry);
		//        }
		//        dc.SubmitChanges();
		//    }
		//}

		#region old variant
		//public static ClientAlertConfig GetAlertConfig(string clientId)
		//{
		//    using (var dc = new StoreDataContext())
		//    {
		//        return dc.ClientAlertConfigs.Where(ac => ac.ClientID == clientId).SingleOrDefault(); 
		//    }
		//}

		//public static void AddAlertConfig(ClientAlertConfig alertConfig)
		//{
		//    using (var dc = new StoreDataContext())
		//    {
		//        dc.ClientAlertConfigs.InsertOnSubmit(alertConfig);
		//        dc.SubmitChanges();
		//    }
		//}

		//public static void UpdateAlertConfig(string clientId, int hourOfPeriod, string statusIDs)
		//{
		//    using (var dc = new StoreDataContext())
		//    {
		//        var alertConfig = GetAlertConfig(clientId);
		//        if (alertConfig == null)
		//            throw new ArgumentNullException("ClientAlertConfig");

		//        alertConfig.HourOfPeriod = hourOfPeriod;
		//        alertConfig.StatusIDs = statusIDs;

		//        dc.SubmitChanges();
		//    }
		//}
		#endregion
	}
}
